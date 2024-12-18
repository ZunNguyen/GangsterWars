using Cysharp.Threading.Tasks;
using Sources.FTUE.GameObject;
using Sources.FTUE.System;
using Sources.GamePlaySystem.MainGamePlay.Enemies;
using Sources.Utils.Singleton;
using UnityEngine;

namespace Sources.FTUE.Command
{
    public class FTUEShowEnemyCommand : FTUECommand
    {
        private FTUESystem _ftueSystem => Locator<FTUESystem>.Instance;

        public override string Description => $"Show Enemy";

        public override async UniTask Execute()
        {
            _ftueSystem.SpawnFTUEEnemy();
        }
    }
}