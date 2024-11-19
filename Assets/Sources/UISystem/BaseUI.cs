using Cysharp.Threading.Tasks;
using DG.Tweening;
using Sirenix.OdinInspector;
using Sources.DataBaseSystem;
using Sources.UISystem;
using Sources.Utils.Singleton;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Sources.UI
{
    public class BaseUI : MonoBehaviour
    {
        [ValueDropdown(nameof(FetchAllUILayers))]
        [SerializeField]
        private string _layer;
        public string Layer => _layer;

        private UIManager _uiManager => Locator<UIManager>.Instance;
        private UITransitionHandler _uiTransitionHandler;
        private string _uiName => GetType().Name;

        protected virtual void Awake()
        {
            _uiTransitionHandler = this.GetComponent<UITransitionHandler>();
        }

        private List<string> FetchAllUILayers()
        {
            return UIData.ActiveUIData.GetUILayers();
        }

        public virtual void OnSetUp(object paramater = null)
        {

        }

        public virtual async UniTask OnTransitionEnter()
        {
            if (_uiTransitionHandler == null) _uiTransitionHandler = GetComponent<UITransitionHandler>();
            await _uiTransitionHandler.DoTransition(true);
        }

        public virtual async UniTask OnTransitionExit()
        {
            await _uiTransitionHandler.DoTransition(false);
        }

        public void Back()
        {
            Close();
        }

        public virtual async UniTask Close()
        {
            await _uiManager.Close(_uiName);
        }

        [Button]
        public void UpdateToUIData()
        {
            UIData.ActiveUIData.AddBaseUI(GetComponent<BaseUI>());
        }
    }
}

