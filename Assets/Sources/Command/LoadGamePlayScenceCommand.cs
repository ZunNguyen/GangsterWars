using Cysharp.Threading.Tasks;
using Game.Screens.BlackLoadingScreen;
using Game.Screens.GamePlayScreen;
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

        public override async UniTask Execute()
        {
            var sequenceGroup = new SequenceServiceCommandGroup("Load Main Game Play Scene");

            var loadingScreen = await _uiManager.Show<BlackLoadingScreen>();
            await loadingScreen.PanelMoveIn();

            await _uiManager.Show<GamePlayScreen>();

            await new InitMainGamePlaySystemService().Execute();
            await new InitLeaderSystemService().Execute();
            await new InitBomberSystemService().Execute();
            await new InitSniperSystemService().Execute();
            sequenceGroup.Add(new LoadSenceCommand("GamePlay"));

            await sequenceGroup.Run();
            await loadingScreen.PanelMoveOut();
            loadingScreen.Close();
        }
    }
}