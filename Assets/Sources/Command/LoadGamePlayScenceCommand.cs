using Cysharp.Threading.Tasks;
using Sources.GamePlaySystem.Character;
using Sources.GamePlaySystem.Leader;
using Sources.GamePlaySystem.MainGamePlay;

namespace Sources.Command
{
    public class LoadGamePlayScenceCommand : Command
    {
        public override async UniTask Execute()
        {
            await new InitLeaderSystemService().Execute();
            await new InitBomberSystemService().Execute();
            await new InitSniperSystemService().Execute();
            await new InitMainGamePlaySystemService().Execute();
            await new LoadSenceCommand("GamePlay").Execute();
        }
    }
}