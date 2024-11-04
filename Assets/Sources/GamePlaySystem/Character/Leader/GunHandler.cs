using Cysharp.Threading.Tasks;
using Sources.DataBaseSystem.Leader;
using Sources.DataBaseSystem;
using Sources.Utils.Singleton;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using System;
using Sources.GameData;

namespace Sources.GamePlaySystem.Leader
{
    public class GunModelView
    {
        public string GunId;
        public string LevelUpgradeId;
        public ReactiveProperty<int> BulletTotal = new ReactiveProperty<int>(0);
        public ReactiveProperty<int> BulletAvailable = new ReactiveProperty<int>(0);
    }

    public class GunHandler
    {
        private const string _gunIdDefault = "gun-01";

        private DataBase _dataBase => Locator<DataBase>.Instance;
        private LeaderConfig _leaderConfig => _dataBase.GetConfig<LeaderConfig>();
        private GameData.GameData _gameData => Locator<GameData.GameData>.Instance;
        private UserProfile _userProfile => _gameData.GetProfileData<UserProfile>();

        private bool _isCanShoot;

        public ReactiveDictionary<string, GunModelView> GunModels { get; private set; } = new();
        public ReactiveProperty<GunModelView> GunModelCurrent { get; private set; } = new();
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
            var gunDatas = _userProfile.LeaderDatas;

            foreach (var gunData in gunDatas)
            {
                var gunModelView = new GunModelView
                {
                    GunId = gunData.WeaponId,
                    LevelUpgradeId = gunData.LevelUpgradeId,
                };

                gunModelView.BulletTotal.Value = gunData.Quatity;

                var gunInfo = _leaderConfig.GetWeaponInfo(gunData.WeaponId);
                gunModelView.BulletAvailable.Value = gunInfo.BulletsPerClip;

                GunModels.Add(gunData.WeaponId, gunModelView);
            }
        }

        private void LoadGunInfoCurrent(string gunId)
        {
            if (GunModels.Remove(gunId, out GunModelView gunModel))
            {
                GunModelCurrent.Value = gunModel;
            }
        }

        private void LoadDamageBulletCurrent()
        {
            var gunInfo = _leaderConfig.GetWeaponInfo(GunModelCurrent.Value.GunId);
            var damageInfo = gunInfo.GetLevelUpgradeInfo(GunModelCurrent.Value.LevelUpgradeId);
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

            _userProfile.Save();
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