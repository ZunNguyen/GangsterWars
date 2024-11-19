using Cysharp.Threading.Tasks;
using Game.Screens.BlackLoadingScreen;
using Game.Screens.GamePlayScreen;
using Game.Screens.JourneyScreen;
using Game.Screens.MainMenuScreen;
using Sources.GamePlaySystem.Character;
using Sources.GamePlaySystem.Leader;
using Sources.GamePlaySystem.MainGamePlay;
using Sources.Services;
using Sources.UISystem;
using Sources.Utils.Singleton;

namespace Sources.Command
{
    public class LoadGamePlayScenceCommand : Command
    {
        private UIManager _uiManager => Locator<UIManager>.Instance;
        private JourneyScreen _journeyScreen => _uiManager.GetUI<JourneyScreen>();
        private MainMenuScreen _mainMenuScreen => _uiManager.GetUI<MainMenuScreen>();

        private string _waveId;

        public LoadGamePlayScenceCommand(string waveId)
        {
            _waveId = waveId;
        }

        public override async UniTask Execute()
        {
            var sequenceGroup = new SequenceServiceCommandGroup("Load Main Game Play Scene");
            sequenceGroup.Add(new LoadSenceCommand("GamePlay"));

            var loadingScreen = await _uiManager.Show<BlackLoadingScreen>();
            await loadingScreen.PanelMoveIn();

            CloseScreen();
            PreOnSetUp();
            
            _uiManager.Show<GamePlayScreen>().Forget();
            sequenceGroup.Run().Forget();

            await loadingScreen.PanelMoveOut();
            loadingScreen.Close().Forget();
        }

        private void CloseScreen()
        {
            _journeyScreen.Close().Forget();
            _mainMenuScreen.Close().Forget();
        }

        private void PreOnSetUp()
        {
            var mainGamePlaySystem = Locator<MainGamePlaySystem>.Instance;
            var leaderSystem = Locator<LeaderSystem>.Instance;
            var bomberSystem = Locator<BomberSystem>.Instance;
            var sniperSystem = Locator<SniperSystem>.Instance;

            mainGamePlaySystem.OnSetUp(_waveId);
            leaderSystem.OnSetUp();
            bomberSystem.OnSetUp();
            sniperSystem.OnSetUp();
        }
    }
}