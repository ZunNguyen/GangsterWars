using Cysharp.Threading.Tasks;
using Sources.GameData;
using Sources.Services;
using Sources.SystemService;
using Sources.Utils.Singleton;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Sources.SaveGame
{
    public class InitGameDataService : Service
    {
        public override async UniTask<IService.Result> Execute()
        {
            GameDataSave gameDataSave = new GameDataSave();
            Locator<GameDataSave>.Set(gameDataSave);

            return IService.Result.Success;
        }
    }
}