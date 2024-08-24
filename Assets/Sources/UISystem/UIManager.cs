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

        public void Initialize()
        {
            InitializeUILayer();
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
        }

        public void Show<T>(object parameter = null) where T : BaseUI
        {
            var uiName = typeof(T).Name;
            Show(uiName);
        }

        public void Show(string uiName)
        {
            if (_uiShowing.ContainsKey(uiName))
            {
                Debug.LogError($"Base UI: {uiName} already showed");
                return;
            }

            CreateUI(uiName);
        }

        private void CreateUI(string uiName)
        {
            var uiPrefab = _uiData.GetBaseUI(uiName);
            if (uiPrefab.SafeIsUnityNull())
            {
                Debug.LogError($"UI: {uiName} doesn't exits in UIData");
                return;
            }

            var layer = GetCorrectLayer(uiPrefab.Layer);
            Instantiate(uiPrefab, _holderPlayer);
        }

        private Transform GetCorrectLayer(string layerName)
        {
            return _layers[layerName];
        }
    }
}