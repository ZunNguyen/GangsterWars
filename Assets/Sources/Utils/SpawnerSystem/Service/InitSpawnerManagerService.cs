using Cysharp.Threading.Tasks;
using Sources.Services;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sources.SpawnerSystem
{
    public class InitSpawnerManagerService : Service
    {
        public override async UniTask<IService.Result> Execute()
        {
            var spawnerManager = new SpawnerManager();
            spawnerManager.Initialize();

            return IService.Result.Success;
        }
    }
}