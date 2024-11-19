using Cysharp.Threading.Tasks;
using Sources.Command;
using Sources.SpawnerSystem;
using Sources.Utils.Singleton;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sources.Command
{
    public class ResetSpawnerManagerCommand : Command
    {
        public override async UniTask Execute()
        {
            var spawnerManager = Locator<SpawnerManager>.Instance;
            if (spawnerManager != null)
            {
                spawnerManager.ResetAllSpawner();
                await UniTask.CompletedTask;
            }
        }
    }
}