using Cysharp.Threading.Tasks;
using Game.Screens.BlackLoadingScreen;
using Game.Screens.GamePlayScreen;
using Game.Screens.JourneyScreen;
using Game.Screens.LoadingScreen;
using Game.Screens.MainMenuScreen;
using Sources.Audio;
using Sources.Command;
using Sources.Extension;
using Sources.GamePlaySystem.MainMenuGame;
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
        private GamePlayScreen _gamePlayScreen => _uiManager.GetUI<GamePlayScreen>();
        private MainMenuScreen _mainMenuScreen => _uiManager.GetUI<MainMenuScreen>();
        private JourneyScreen _journeyScreen => _uiManager.GetUI<JourneyScreen>();

        private StoreSystem _storeSystem => Locator<StoreSystem>.Instance;

        public override async UniTask Execute()
        {
            var sequenceGroup = new SequenceServiceCommandGroup("Load Main Menu Scene");

            sequenceGroup.Add(new LoadSenceCommand(NameScenceKey.NAME_SCENCE_MAIN_MENU));

            var loadingScreen = await _uiManager.Show<BlackLoadingScreen>();
            
            await loadingScreen.PanelMoveIn();
            _storeSystem.OnSetUp();
            CloseScreen();
            await sequenceGroup.Run();
            Time.timeScale = 1f;
            _uiManager.Show<MainMenuScreen>().Forget();
            await loadingScreen.PanelMoveOut();
            
            loadingScreen.Close().Forget();
        }

        private void CloseScreen()
        {
            if (_gamePlayScreen != null) _gamePlayScreen.Close().Forget();
            if (_mainMenuScreen != null) _mainMenuScreen.Close().Forget();
            if (_journeyScreen != null) _journeyScreen.Close().Forget();
        }
    }
}