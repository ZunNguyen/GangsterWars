using Cysharp.Threading.Tasks;
using Game.Screens.BlackLoadingScreen;
using Game.Screens.LoadingScreen;
using Game.Screens.MainMenuScreen;
using Sources.Command;
using Sources.Services;
using Sources.UISystem;
using Sources.Utils.Singleton;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sources.Command
{
    public class LoadMainMenuScenceCommand : Command
    {
        private UIManager _uiManager => Locator<UIManager>.Instance;

        public override async UniTask Execute()
        {
            var sequenceGroup = new SequenceServiceCommandGroup("Load Main Menu Scene");

            sequenceGroup.Add(new LoadSenceCommand("MainMenu"));

            var loadingScreen = await _uiManager.Show<BlackLoadingScreen>();
            await loadingScreen.PanelMoveIn();
            _uiManager.Show<MainMenuScreen>();
            await sequenceGroup.Run();
            loadingScreen.Close();
        }
    }
}