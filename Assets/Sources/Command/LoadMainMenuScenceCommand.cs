using Cysharp.Threading.Tasks;
using Game.Screens.BlackLoadingScreen;
using Game.Screens.GamePlayScreen;
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
        private AudioManager _audioManager => Locator<AudioManager>.Instance;

        private UIManager _uiManager => Locator<UIManager>.Instance;
        private GamePlayScreen _gamePlayScreen => _uiManager.GetUI<GamePlayScreen>();
        private StoreSystem _storeSystem => Locator<StoreSystem>.Instance;

        public override async UniTask Execute()
        {
            var sequenceGroup = new SequenceServiceCommandGroup("Load Main Menu Scene");

            sequenceGroup.Add(new LoadSenceCommand("MainMenu"));

            var loadingScreen = await _uiManager.Show<BlackLoadingScreen>();
            _audioManager.Play(AudioKey.MENU_SONG, true);

            await loadingScreen.PanelMoveIn();
            await _storeSystem.Init();
            if (_gamePlayScreen != null) _gamePlayScreen.Close().Forget();
            await sequenceGroup.Run();
            _uiManager.Show<MainMenuScreen>().Forget();
            await loadingScreen.PanelMoveOut();
            loadingScreen.Close().Forget();
        }
    }
}