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

        [SerializeField, ValueDropdown(nameof(GetAllLanguageIds))]
        private string _languageId;
        private IEnumerable GetAllLanguageIds => IdGetter.GetAllLanguageIds();

        public override string Description => $"Show language id: {_languageId}";

        public override async UniTask Execute()
        {
            var uiFTUEScreen = _uiManager.GetUIShowing<FTUEScreen>();
            if (uiFTUEScreen == null)
            {
                Debug.Log($"UI {uiFTUEScreen.name} not yet show");
                return;
            }

            var ftueViewModels = uiFTUEScreen.GetComponentsInChildren<IFTUEVieModel>();
            FTUELanguageText ftueLanguageText = new ();
            foreach (var model in ftueViewModels)
            {
                if (model is FTUELanguageText FTUELanguageText)
                {
                    ftueLanguageText = FTUELanguageText;
                    ftueLanguageText.OnSetUp(_languageId);
                    return;
                }
                else
                {
                    Debug.Log($"FTUE Language Text not yet show");
                    return;
                }
            }
        }
    }
}