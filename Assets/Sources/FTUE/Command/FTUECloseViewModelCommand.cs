using Cysharp.Threading.Tasks;
using Game.Screens.FTUEScreen;
using Sirenix.OdinInspector;
using Sources.FTUE.GameObject;
using Sources.UISystem;
using Sources.Utils.Singleton;
using Sources.Utils;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Sources.FTUE.Command
{
    public class FTUECloseViewModelCommand : FTUECommand
    {
        private UIManager _uiManager => Locator<UIManager>.Instance;

        [SerializeField, ValueDropdown(nameof(_getAllUIName))]
        private string _uiTypeName;
        private IEnumerable _getAllUIName => IdGetter.GetAllUIName();

        [SerializeField, ValueDropdown(nameof(_getAllFTUEKey))]
        private List<string> _ftueViewModelKeysClose;

        public override string Description => $"Close FTUE view model: {_ftueViewModelKeysClose[0]}";

        public override async UniTask Execute()
        {
            var uiTypeName = _uiManager.GetUIShowing(_uiTypeName);
            if (uiTypeName == null)
            {
                Debug.Log($"UI {_uiTypeName} not yet show");
                return;
            }

            var uiFTUEScreen = _uiManager.GetUIShowing<FTUEScreen>() as FTUEScreen;
            if (uiFTUEScreen == null)
            {
                Debug.Log($"UI {uiFTUEScreen.name} not yet show");
                return;
            }

            var ftueViewModels = uiTypeName.GetComponentsInChildren<FTUEViewModel>();
            var ftueViewModelsChoosed = new List<FTUEViewModel>();
            foreach (var ftueViewModel in ftueViewModels)
            {
                var isChoosed = _ftueViewModelKeysClose.Any(x => ftueViewModel.FTUEViewModelKey.Contains(x));
                if (isChoosed) ftueViewModelsChoosed.Add(ftueViewModel);
            }

            foreach (var ftueViewModel in ftueViewModelsChoosed)
            {
                ftueViewModel.BackViewModelOrigin();
            }
        }
    }
}