using Cysharp.Threading.Tasks;
using Game.Screens.FTUEScreen;
using Sirenix.OdinInspector;
using Sources.FTUE.GameObject;
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

        [SerializeField, ValueDropdown(nameof(_getAllUIName))] 
        private string _uiTypeName;
        private IEnumerable _getAllUIName => IdGetter.GetAllUIName();

        [SerializeField, ValueDropdown(nameof(_getAllFTUEKey))]
        private List<string> _ftueViewModelKeys;

        public override string Description => $"Show FTUE view model in {_uiTypeName}";

        public override async UniTask Execute()
        {
            var uiTypeName = _uiManager.GetUIShowing(_uiTypeName);
            if (uiTypeName == null)
            {
                Debug.Log($"UI {_uiTypeName} not yet show");
                return;
            }

            var uiFTUEScreen = _uiManager.GetUIShowing<FTUEScreen>();
            if (uiFTUEScreen == null)
            {
                Debug.Log($"UI {uiFTUEScreen.name} not yet show");
                return;
            }

            var ftueViewModels = uiTypeName.GetComponentsInChildren<IFTUEVieModel>();
            var ftueViewModelsChoosed = new List<IFTUEVieModel>();
            foreach (var ftueViewModel in ftueViewModels)
            {
                var isChoosed = _ftueViewModelKeys.Any(x => ftueViewModel.FTUEViewModelKey.Contains(x));
                if (isChoosed) ftueViewModelsChoosed.Add(ftueViewModel);
            }
        }
    }
}