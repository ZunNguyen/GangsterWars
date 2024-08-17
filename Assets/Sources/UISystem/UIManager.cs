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


        public List<BaseUI> GetAllShowedUIs()
        {
            return _showedUI.Values.ToList();
        }

        public void AddToCloseAllExceptList(string ui)
        {
            _closeAllExcepts ??= new HashSet<string>();

            _closeAllExcepts.Add(ui);
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