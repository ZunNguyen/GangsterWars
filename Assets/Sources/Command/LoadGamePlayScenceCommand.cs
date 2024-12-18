using Cysharp.Threading.Tasks;
using Game.Screens.BlackLoadingScreen;
using Game.Screens.GamePlayScreen;
using Game.Screens.JourneyScreen;
using Game.Screens.MainMenuScreen;
using Sources.Audio;
using Sources.Extension;
using Sources.GamePlaySystem.Character;
using Sources.GamePlaySystem.GameResult;
using Sources.GamePlaySystem.Leader;
using Sources.GamePlaySystem.MainGamePlay;
using Sources.Services;
using Sources.UISystem;
using Sources.Utils.Singleton;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sources.Command
{
    public class LoadGamePlayScenceCommand : Command
    {
        private UIManager _uiManager => Locator<UIManager>.Instance;
        private JourneyScreen _journeyScreen => _uiManager.GetUI<JourneyScreen>();
        private MainMenuScreen _mainMenuScreen => _uiManager.GetUI<MainMenuScreen>();
        private GamePlayScreen _gamePlayScreen => _uiManager.GetUI<GamePlayScreen>();
        private AudioManager _audioManager = Locator<AudioManager>.Instance;

        private string _waveId;

        public LoadGamePlayScenceCommand(string waveId)
        {
            _waveId = waveId;
        }

        public override async UniTask Execute()
        {
            var sequenceGroup = new SequenceServiceCommandGroup("Load Main Game Play Scene");
            sequenceGroup.Add(new LoadSenceCommand(NameScenceKey.NAME_SCENCE_GAME_PLAY));


            var loadingScreen = await _uiManager.Show<BlackLoadingScreen>();
            await loadingScreen.PanelMoveIn();
            PreOnSetUp();
            await CloseScreen();
            
            _uiManager.Show<GamePlayScreen>().Forget();
            await sequenceGroup.Run();

            _audioManager.Play(AudioKey.MENU_SONG, true);

            await loadingScreen.PanelMoveOut();
            loadingScreen.Close().Forget();
        }

        private async UniTask CloseScreen()
        {
            await UniTask.WhenAll(_journeyScreen.Close(),
                                    _mainMenuScreen.Close(), _gamePlayScreen.Close());
        }       

        private void PreOnSetUp()
        {
            var mainGamePlaySystem = Locator<MainGamePlaySystem>.Instance;
            var leaderSystem = Locator<LeaderSystem>.Instance;
            var bomberSystem = Locator<BomberSystem>.Instance;
            var sniperSystem = Locator<SniperSystem>.Instance;
            var gameResultSystem = Locator<GameResultSystem>.Instance;

            mainGamePlaySystem.OnSetUp(_waveId);
            leaderSystem.OnSetUp();
            bomberSystem.OnSetUp();
            sniperSystem.OnSetUp();
            gameResultSystem.OnSetUp();
        }
    }
}