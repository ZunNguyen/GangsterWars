using Cysharp.Threading.Tasks;
using Game.Screens.FTUEScreen;
using Sirenix.OdinInspector;
using Sources.FTUE.GameObject;
using Sources.UISystem;
using Sources.Utils;
using Sources.Utils.Singleton;
using System.Collections;
using UnityEngine;

namespace Sources.FTUE.Command
{
    public class FTUEShowTextCommand : FTUECommand
    {
        private UIManager _uiManager => Locator<UIManager>.Instance;

        [SerializeField, ValueDropdown(nameof(_getAllUIName))]
        private string _uiTypeName;
        private IEnumerable _getAllUIName => IdGetter.GetAllUIName();

        [SerializeField, ValueDropdown(nameof(GetAllLanguageIds))]
        private string _languageId;
        private IEnumerable GetAllLanguageIds => IdGetter.GetAllLanguageIds();

        public override string Description => $"Show language id: {_languageId}";

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

            var ftueViewModels = uiTypeName.GetComponentsInChildren<FTUEViewModel>();
            foreach (var model in ftueViewModels)
            {
                foreach (var objectData in model.ObjectDatas)
                {
                    if (objectData.IsLanguageText)
                    {
                        model.SetLanguage(_languageId);
                    }
                }
            }
        }
    }
}