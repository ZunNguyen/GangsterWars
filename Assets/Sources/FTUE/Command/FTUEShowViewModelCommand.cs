using Cysharp.Threading.Tasks;
using Game.Screens.FTUEScreen;
using Sirenix.OdinInspector;
using Sources.FTUE.GameObject;
using Sources.UI;
using Sources.UISystem;
using Sources.Utils;
using Sources.Utils.Singleton;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Sources.FTUE.Command
{
    [Serializable]
    public class FTUEShowViewModelCommand : FTUECommand
    {
        private UIManager _uiManager => Locator<UIManager>.Instance;

        [SerializeField] private bool _isShow;
        private bool _isClose => !_isShow;

        [SerializeField, ValueDropdown(nameof(_getAllUIName)), ShowIf(nameof(_isShow))] 
        private string _uiTypeName;
        private IEnumerable _getAllUIName => IdGetter.GetAllUIName();

        [SerializeField, ValueDropdown(nameof(_getAllFTUEKey)), ShowIf(nameof(_isShow))]
        private List<string> _ftueViewModelKeysShow;

        [SerializeField, ValueDropdown(nameof(_getAllFTUEKey)), ShowIf(nameof(_isClose))]
        private List<string> _ftueViewModelKeysClose;

        public override string Description => $"Show FTUE view model in {_uiTypeName}";

        public override async UniTask Execute()
        {
            var uiTypeName = _uiManager.GetUIShowing(this._uiTypeName);
            if (uiTypeName == null)
            {
                Debug.Log($"UI {this._uiTypeName} not yet show");
                return;
            }

            var uiFTUEScreen = _uiManager.GetUIShowing<FTUEScreen>() as FTUEScreen;
            if (uiFTUEScreen == null)
            {
                Debug.Log($"UI {uiFTUEScreen.name} not yet show");
                return;
            }

            if (_isShow) IsShow();
            else IsClose();

            void IsShow()
            {
                var ftueViewModels = uiTypeName.GetComponentsInChildren<FTUEViewModel>();
                var ftueViewModelsChoosed = new List<FTUEViewModel>();
                foreach (var ftueViewModel in ftueViewModels)
                {
                    var isChoosed = _ftueViewModelKeysShow.Any(x => ftueViewModel.FTUEViewModelKey.Contains(x));
                    if (isChoosed) ftueViewModelsChoosed.Add(ftueViewModel);
                }

                uiFTUEScreen.SetUpViewModel(ftueViewModelsChoosed);
            }

            void IsClose()
            {
                var ftueViewModels = uiFTUEScreen.GetComponentsInChildren<FTUEViewModel>();
                var ftueViewModelsChoosed = new List<FTUEViewModel>();
                foreach (var ftueViewModel in ftueViewModels)
                {
                    var isChoosed = _ftueViewModelKeysShow.Any(x => ftueViewModel.FTUEViewModelKey.Contains(x));
                    if (isChoosed) ftueViewModelsChoosed.Add(ftueViewModel);
                }

                foreach (var ftueViewModel in ftueViewModelsChoosed)
                {
                    ftueViewModel.BackViewModelOrigin();
                }
            }
        }
    }
}