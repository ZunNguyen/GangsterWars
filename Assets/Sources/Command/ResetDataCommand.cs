using Cysharp.Threading.Tasks;
using Game.Screens.GamePlayScreen;
using Game.Screens.JourneyScreen;
using Game.Screens.MainMenuScreen;
using Sources.GamePlaySystem.MainMenuGame;
using Sources.UISystem;
using Sources.Utils;
using Sources.Utils.Singleton;

namespace Sources.Command
{
    public class ResetDataCommand : Command
    {
        private UIManager _uiManager => Locator<UIManager>.Instance;
        private MainMenuScreen _mainMenuScreen => _uiManager.GetUI<MainMenuScreen>();
        private JourneyScreen _journeyScreen => _uiManager.GetUI<JourneyScreen>();

        public override async UniTask Execute()
        {
            GameDataUtils.ClearData();
            await UniTask.DelayFrame(5);
            new LoadMainMenuScenceCommand().Execute();
        }
    }
}