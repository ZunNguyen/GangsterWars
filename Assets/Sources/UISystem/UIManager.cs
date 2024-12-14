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
        private Dictionary<string, BaseUI> _uiShowing = new Dictionary<string, BaseUI>();
        private Dictionary<string, Transform> _layers = new Dictionary<string, Transform>();

        [SerializeField] private GameObject _layerPrefab;
        [SerializeField] private Transform _holderPlayer;
        [SerializeField] private Transform _mainCamera;

        public void Init()
        {
            InitializeUILayer();
            InitializeCamera();
        }

        private void InitializeUILayer()
        {
            var uiLayers = _uiData.GetUILayers();

            foreach (var uiLayer in uiLayers)
            {
                var newLayer = Instantiate(_layerPrefab, _holderPlayer);
                newLayer.name = uiLayer;

                _layers.Add(uiLayer, newLayer.transform);
            }

            _layers.Count();
        }

        private void InitializeCamera()
        {
            var mainCamera = Instantiate(_mainCamera);
            mainCamera.name = "UI Camera";
            DontDestroyOnLoad(mainCamera);
        }

        public async UniTask<T> Show<T>(object parameter = null) where T : BaseUI
        {
            var uiName = typeof(T).Name;
            return await Show(uiName, parameter) as T;
        }

        public async UniTask<BaseUI> Show(string uiName, object parameter = null)
        {
            if (_uiShowing.ContainsKey(uiName))
            {
                Debug.LogError($"Base UI: {uiName} already showed");
                return null;
            }

            var ui = CreateUI(uiName);
            if (!ui.SafeIsUnityNull())
            {
                ui.OnSetUp(parameter);
                await ui.OnTransitionEnter();
            }
            _uiShowing.Add(uiName, ui);

            return ui;
        }

        private BaseUI CreateUI(string uiName)
        {
            var uiPrefab = _uiData.GetBaseUI(uiName);
            if (uiPrefab.SafeIsUnityNull())
            {
                Debug.LogError($"UI: {uiName} doesn't exits in UIData");
                return null;
            }

            var layer = GetCorrectLayer(uiPrefab.Layer);
            return Instantiate(uiPrefab, layer);
        }

        private Transform GetCorrectLayer(string layerName)
        {
            var layer = _layers[layerName];
            if (layer == null) Debug.LogWarning($"Don't find correct layer");
            return _layers[layerName];
        }

        public async UniTask Close<T>() where T : BaseUI
        {
            var uiName = typeof(T).Name;
            await Close(uiName);
        }

        public async UniTask Close(string uiName)
        {
            if (_uiShowing.TryGetValue(uiName, out BaseUI ui))
            {
                _uiShowing.Remove(uiName);
                if (ui.SafeIsUnityNull()) return;

                await ui.OnTransitionExit();
                DestroyUI(ui);
            }
            else
            {
                Debug.Log($"UI {uiName} aldready closed");
            }
        }

        public T GetUI<T>() where T : BaseUI
        {
            var uiName = typeof(T).Name;
            return GetUI(uiName) as T;
        }

        public BaseUI GetUI(string uiName)
        {
            return _uiData.GetBaseUI(uiName);
        }

        public BaseUI GetUIShowing<T>() where T : BaseUI
        {
            var uiName = typeof(T).Name;
            return GetUIShowing(uiName);
        }

        public BaseUI GetUIShowing(string uiName)
        {
            if (_uiShowing.TryGetValue(uiName, out BaseUI ui)) return ui;
            return null;
        }

        private void DestroyUI(BaseUI ui)
        {
            Destroy(ui.gameObject);
        }
    }
}