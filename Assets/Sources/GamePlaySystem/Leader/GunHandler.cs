using Cysharp.Threading.Tasks;
using Sources.DataBaseSystem.Leader;
using Sources.DataBaseSystem;
using Sources.Utils.Singleton;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using System;

namespace Sources.GamePlaySystem.Leader
{
    public class GunHandler
    {
        private const string _gunIdDefault = "gun-01";

        private DataBase _dataBase => Locator<DataBase>.Instance;
        private LeaderConfig _leaderConfig => _dataBase.GetConfig<LeaderConfig>();
        private GameData.GameData _gameData => Locator<GameData.GameData>.Instance;

        public ReactiveDictionary<string, GunModel> GunModels { get; private set; } = new();
        public ReactiveProperty<GunModel> GunModelCurrent { get; private set; } = new();
        public ReactiveProperty<bool> IsCanShoot { get; private set; } = new();

        public Action Shooting;

        public void OnSetUp()
        {
            LoadGunModels();
            LoadGunCurrent(_gunIdDefault);
            CheckCanShoot();
        }

        private void LoadGunModels()
        {
            var gunModels = _gameData.SetDataLeaderData();

            foreach (var gunModel in gunModels)
            {
                var key = gunModel.GunId;
                var gunInfo = _leaderConfig.GetWeaponInfo(key);
                gunModel.BulletAvailable.Value = gunInfo.BulletsPerClip;

                GunModels.Add(key, gunModel);
            }
        }

        private void LoadGunCurrent(string gunId)
        {
            if (GunModels.Remove(gunId, out GunModel gunModel))
            {
                GunModelCurrent.Value = gunModel;
            }
        }

        private void CheckCanShoot()
        {
            var bulletCurrent = GunModelCurrent.Value.BulletAvailable.Value;
            IsCanShoot.Value = bulletCurrent > 0;
        }

        public void UpdateBullet()
        {
            if (GunModelCurrent.Value.BulletTotal.Value != 0)
            {
                GunModelCurrent.Value.BulletAvailable.Value -= 1;
                GunModelCurrent.Value.BulletTotal.Value -= 1;
            }

            CheckCanShoot();
            Shooting?.Invoke();
        }

        public void ChangeGunModel(string gunId)
        {
            GunModels.Add(GunModelCurrent.Value.GunId, GunModelCurrent.Value);
            LoadGunCurrent(gunId);
            CheckCanShoot();
        }

        public void AddBullet()
        {
            if (GunModelCurrent.Value.BulletTotal.Value != 0)
            {
                GunModelCurrent.Value.BulletAvailable.Value += 1;

                Debug.Log($"BulletAvailable: {GunModelCurrent.Value.BulletAvailable.Value}");
            }
            
            CheckCanShoot();
        }
    }
}