using Cysharp.Threading.Tasks;
using Sources.DataBaseSystem;
using Sources.DataBaseSystem.Leader;
using Sources.SystemService;
using Sources.Utils.Singleton;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sources.GamePlaySystem.Leader
{
    public class InitLeaderSystemService : InitSystemService<LeaderSystem> { };

    public class GunModel
    {
        public string GunId;
        public string LevelDamage;
    }

    public class LeaderSystem : BaseSystem
    {
        private const string _levelDamageDefault = "level-0";

        private DataBase _dataBase => Locator<DataBase>.Instance;
        private LeaderConfig _leaderConfig => _dataBase.GetConfig<LeaderConfig>();

        public Action<GunModel> GunModel;

        public override async UniTask Init()
        {
            LoadGunModel();
        }

        private void LoadGunModel()
        {
            var gunModel = new GunModel
            {
                GunId = _leaderConfig.Weapons[0].Id,
                LevelDamage = _levelDamageDefault
            };

            GunModel?.Invoke(gunModel);
        }

        public void UpDateGunModel(GunModel gunModel)
        {
            GunModel?.Invoke(gunModel);
        }
    }
}