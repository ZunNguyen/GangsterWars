using Cysharp.Threading.Tasks;
using Sources.GameData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sources.Services
{
    public class InitGameDataService : Service
    {
        public override async UniTask<IService.Result> Execute()
        {
            GameData.GameData gameData = new();
            gameData.Init();

            return IService.Result.Success;
        }
    }
}