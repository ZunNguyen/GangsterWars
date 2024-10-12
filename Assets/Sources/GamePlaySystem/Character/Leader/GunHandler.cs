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
        private bool _isCanShoot;

        public ReactiveDictionary<string, GunModel> GunModels { get; private set; } = new();
        public ReactiveProperty<GunModel> GunModelCurrent { get; private set; } = new();
        public Action IsShooting;
        public int DamageBulletCurrent { get; private set; }

        public void OnSetUp()
        {
            LoadGunModels();
            LoadGunInfoCurrent(_gunIdDefault);
            CheckCanShoot();
            LoadDamageBulletCurrent();
        }

        private void LoadGunModels()
        {
            var gunModels = _gameData.UserData.SetDataLeaderData();

            foreach (var gunModel in gunModels)
            {
                var key = gunModel.GunId;
                var gunInfo = _leaderConfig.GetWeaponInfo(key);
                gunModel.BulletAvailable.Value = gunInfo.BulletsPerClip;

                GunModels.Add(key, gunModel);
            }
        }

        private void LoadGunInfoCurrent(string gunId)
        {
            if (GunModels.Remove(gunId, out GunModel gunModel))
            {
                GunModelCurrent.Value = gunModel;
            }
        }

        private void LoadDamageBulletCurrent()
        {
            var gunInfo = _leaderConfig.GetWeaponInfo(GunModelCurrent.Value.GunId);
            var damageInfo = gunInfo.GetDamageWeapon(GunModelCurrent.Value.LevelDamage);
            DamageBulletCurrent = damageInfo.Damage;
        }

        private void CheckCanShoot()
        {
            var bulletCurrent = GunModelCurrent.Value.BulletAvailable.Value;
            _isCanShoot = bulletCurrent > 0;
        }

        public void SubtractBullet()
        {
            if (GunModelCurrent.Value.BulletTotal.Value != 0)
            {
                GunModelCurrent.Value.BulletAvailable.Value -= 1;
                GunModelCurrent.Value.BulletTotal.Value -= 1;
                IsShooting?.Invoke();
            }

            CheckCanShoot();
        }

        public void ChangeGunModel(string gunId)
        {
            GunModels.Add(GunModelCurrent.Value.GunId, GunModelCurrent.Value);
            LoadGunInfoCurrent(gunId);
            CheckCanShoot();
            LoadDamageBulletCurrent();
        }

        public void AddBulletAvailable()
        {
            if (GunModelCurrent.Value.BulletTotal.Value > 0)
            {
                GunModelCurrent.Value.BulletAvailable.Value += 1;

                Debug.Log($"BulletAvailable: {GunModelCurrent.Value.BulletAvailable.Value}");
            }
            
            CheckCanShoot();
        }

        public void Shooting()
        {
            if (!_isCanShoot) return;
            SubtractBullet();
        }
    }
}