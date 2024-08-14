using Cysharp.Threading.Tasks;
using Sirenix.Utilities;
using Sources.UI;
using Sources.Utils.Singleton;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace Sources.UISystem
{
    public class UIManager : MonoBehaviour
    {
        private UIData _uiData => Locator<UIData>.Instance;

        [SerializeField] private GameObject _cameraPrefab;
        [SerializeField] private Transform layerRoot;
        [SerializeField] private GameObject layerPrefab;
        [SerializeField] private Canvas canvas;

        private Dictionary<string, Transform> _uiLayerTransformRoots;
        private Dictionary<string, BaseUI> _showedUI;

        private HashSet<string> _closeAllExcepts = new HashSet<string>();

        public Camera UICamera { get; private set; }

        // events
        public Action<string> OnBeginShowUI;
        public Action<string> OnEndShowUI;

        public Action<string> OnBeginCloseUI;
        public Action<string> OnEndCloseUI;

        private void Awake()
        {
            _uiLayerTransformRoots = new Dictionary<string, Transform>();
            _showedUI = new Dictionary<string, BaseUI>();
        }

        public void Init()
        {
            InitUICamera();
            InitializeUILayer();
        }

        public void InitializeUILayer()
        {
            _uiLayerTransformRoots ??= new Dictionary<string, Transform>();
            var uiLayers = _uiData.GetUILayers();
            foreach (var uiLayer in uiLayers)
            {
                if (_uiLayerTransformRoots.ContainsKey(uiLayer)) continue;

                var layerTransform = Instantiate(layerPrefab, layerRoot);
                layerTransform.name = uiLayer;

                _uiLayerTransformRoots.Add(uiLayer, layerTransform.transform);
            }
        }

        public async UniTask TemporaryHideAllUIs(float duration = 0f)
        {
            var tasks = new List<UniTask>();
            foreach (var baseUI in _showedUI.Values)
            {
                tasks.Add(baseUI.TemporaryHide(duration));
            }

            await UniTask.WhenAll(tasks);
        }

        public async UniTask TemporaryShowAllUIs(float duration = 0f)
        {
            var tasks = new List<UniTask>();
            foreach (var baseUI in _showedUI.Values)
            {
                tasks.Add(baseUI.TemporaryShow(duration));
            }

            await UniTask.WhenAll(tasks);
        }

        public void CloseAllCurrentShowed()
        {
            IEnumerable<string> keys = _showedUI.Keys.ToList();
            if (_closeAllExcepts != null)
                keys = keys.Where(x => !_closeAllExcepts.Contains(x));

            foreach (var key in keys)
            {
#pragma warning disable CS4014
                ForceClose(key);
#pragma warning restore CS4014
            }
        }

        public List<BaseUI> GetAllShowedUIs()
        {
            return _showedUI.Values.ToList();
        }

        public void AddToCloseAllExceptList(string ui)
        {
            _closeAllExcepts ??= new HashSet<string>();

            _closeAllExcepts.Add(ui);
        }

        public async UniTask<T> Show<T>(object parameter = null, Action<object> onCloseCallBack = null) where T : BaseUI
        {
            string uiName = typeof(T).ToString();
            return await Show(uiName, parameter, onCloseCallBack) as T;
        }

        public T GetShowedUI<T>() where T : BaseUI
        {
            string uiName = typeof(T).ToString();
            return GetShowedUI(uiName) as T;
        }

        public async UniTask<T> WaitAndGetShowedUI<T>() where T : BaseUI
        {
            string uiName = typeof(T).ToString();

            var ui = GetShowedUI(uiName);
            if (!ui.SafeIsUnityNull())
            {
                return ui as T;
            }

            await UniTask.DelayFrame(1);
            return await WaitAndGetShowedUI<T>();
        }

        public BaseUI GetShowedUI(Type uiType)
        {
            string uiName = uiType.ToString();
            return GetShowedUI(uiName);
        }

        public BaseUI GetShowedUI(string uiName)
        {
            if (_showedUI.TryGetValue(uiName, out BaseUI ui))
            {
                return ui;
            }

            return null;
        }

        public async UniTask<BaseUI> Show(string uiName, object parameter = null, Action<object> onCloseCallBack = null)
        {
            if (_showedUI.ContainsKey(uiName))
            {
                Debug.LogError($"UI: {uiName} already showed.");
                return null;
            }

            BaseUI ui = CreateUI(uiName);
            ui.OnClose = onCloseCallBack;

            if (!ui.SafeIsUnityNull())
            {
                _showedUI.Add(ui.uiName, ui);

                OnBeginShowUI?.Invoke(ui.uiName);

                //MessageBroker.Default.Publish(AnalyticEvent.Create(AnalyticKey.Event.UI.Show, ui.uiNameOnly));

                ui.OnSetup(@parameter);
                await ui.OnTransitionEnter(@parameter);
                ui.OnShow(@parameter);

                OnEndShowUI?.Invoke(ui.uiName);

                //MessageBroker.Default.Publish(TrackingEventData.Create(TrackingKey.UIShowed,
                //    new UICustomData()
                //    {
                //        UIName = ui.GetType().Name,
                //    })
                //);
            }

            return ui;
        }

        public async UniTask<BaseUI> Show(Type type, object parameter = null, Action<object> onCloseCallBack = null)
        {
            string uiName = type.ToString();
            return await Show(uiName, parameter, onCloseCallBack);
        }

        public async UniTask Close<T>(object param = null) where T : BaseUI
        {
            string uiName = typeof(T).ToString();
            await Close(uiName, param);
        }

        public async UniTask Close(string uiName, object param = null)
        {
            if (_showedUI.TryGetValue(uiName, out BaseUI ui))
            {
                _showedUI.Remove(uiName);
                if (ui.SafeIsUnityNull()) return;

                OnBeginCloseUI?.Invoke(uiName);

                await ui.OnTransitionExit();
                ui.OnClose?.Invoke(param);
                ui.OnClose = null;

                //MessageBroker.Default.Publish(TrackingEventData.Create(TrackingKey.UIClosed,
                //    new UICustomData()
                //    {
                //        UIName = ui.GetType().Name,
                //    })
                //);

                DestroyUI(ui);

                OnEndCloseUI?.Invoke(uiName);
                await UniTask.CompletedTask;
            }
            else
            {
                Debug.LogError($"UI: {uiName} already closed");
            }
        }

        // Close without transitionExit
        public async UniTask ForceClose<T>(object param = null) where T : BaseUI
        {
            string uiName = typeof(T).ToString();
            await ForceClose(uiName, param);
        }

        public async UniTask ForceClose(string uiName, object param = null)
        {
            if (_showedUI.TryGetValue(uiName, out BaseUI ui))
            {
                _showedUI.Remove(uiName);
                if (ui.SafeIsUnityNull()) return;

                await UniTask.CompletedTask;
                OnBeginCloseUI?.Invoke(uiName);
                ui.OnClose?.Invoke(param);
                ui.OnClose = null;

                //MessageBroker.Default.Publish(TrackingEventData.Create(TrackingKey.UIClosed,
                //    new UICustomData()
                //    {
                //        UIName = ui.GetType().Name,
                //    })
                //);

                DestroyUI(ui);
                OnEndCloseUI?.Invoke(uiName);
            }
            else
            {
                Debug.LogError($"UI: {uiName} already closed");
            }
        }

        public BaseUI CreateUI(string uiName)
        {
            BaseUI uiPrefab = _uiData.Get(uiName);
            if (uiPrefab.SafeIsUnityNull())
            {
                Debug.LogError($"UI: {uiName} doesn't exists in UIData. Please investigate!");
                return null;
            }

            var layerRoot = GetCorrectLayer(uiPrefab.Layer);
            if (layerRoot.SafeIsUnityNull())
            {
                Debug.LogError($"uiPrefab.Layer: {uiPrefab.Layer} doesn't exists in UIData. Please investigate!");
                return null;
            }

            return Instantiate(uiPrefab, layerRoot);
        }

        public void DestroyUI(BaseUI ui)
        {
            if (ui.SafeIsUnityNull())
                Debug.LogError($"UI: {nameof(ui)} is null");
            else
                Destroy(ui.gameObject);
        }

        public void InitUICamera()
        {
            var uiCamera = Instantiate(_cameraPrefab);
            uiCamera.name = "UI Camera";
            DontDestroyOnLoad(uiCamera);
            _cameraPrefab.transform.position = Vector3.zero;

            canvas.worldCamera = uiCamera.GetComponentInChildren<Camera>();
            canvas.sortingLayerName = "UI";
            UICamera = canvas.worldCamera;
        }

        public Transform GetCorrectLayer(string layerName)
        {
            if (!_uiLayerTransformRoots.TryGetValue(layerName, out var layerTransform))
            {
                InitializeUILayer();
            }

            return _uiLayerTransformRoots[layerName];
        }
    }
}