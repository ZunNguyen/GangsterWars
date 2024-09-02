using Cysharp.Threading.Tasks;
using Sources.DataBaseSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sources.Services
{
    public class InitDataBaseService : Service
    {
        private DataBase _dataBase;

        public InitDataBaseService(DataBase dataBase)
        {
            _dataBase = dataBase;
        }

        public override async UniTask<IService.Result> Execute()
        {
            _dataBase.Init();
            return IService.Result.Success;
        }
    }
}

