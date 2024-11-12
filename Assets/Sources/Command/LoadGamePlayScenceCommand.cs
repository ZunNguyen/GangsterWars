using Cysharp.Threading.Tasks;
using Sources.GamePlaySystem.Bomber;
using Sources.GamePlaySystem.Leader;
using Sources.GamePlaySystem.MainGamePlay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sources.Command
{
    public class LoadGamePlayScenceCommand : Command
    {
        public override async UniTask Execute()
        {
            await new InitLeaderSystemService().Execute();
            await new InitBomberSystemService().Execute();
            await new InitMainGamePlaySystemService().Execute();
            await new LoadSenceCommand("GamePlay").Execute();
        }
    }
}