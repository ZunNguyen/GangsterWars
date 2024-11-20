using Cysharp.Threading.Tasks;
using Game.Screens.BlackLoadingScreen;
using Game.Screens.JourneyScreen;
using Sources.GamePlaySystem.JourneyMap;
using Sources.UISystem;
using Sources.Utils.Singleton;

namespace Sources.Command
{
    public class LoadJourneyMapCommand : Command
    {
        private UIManager _uiManager => Locator<UIManager>.Instance;
        private JourneyScreen _journeyScreen => _uiManager.GetUI<JourneyScreen>();

        private JourneyMapSystem _journeyMapSystem => Locator<JourneyMapSystem>.Instance;

        private int _indexJourneyMap;

        public LoadJourneyMapCommand(int indexJourneyMap)
        {
            _indexJourneyMap = indexJourneyMap;
        }

        public override async UniTask Execute()
        {
            var blackLoadingScreen = await _uiManager.Show<BlackLoadingScreen>();
            await blackLoadingScreen.PanelMoveIn();
            _journeyScreen.Back();
            _journeyMapSystem.ChangeJourneyMap(_indexJourneyMap);
            await _uiManager.Show<JourneyScreen>();
            await blackLoadingScreen.PanelMoveOut();
            blackLoadingScreen.Back();
        }
    }
}