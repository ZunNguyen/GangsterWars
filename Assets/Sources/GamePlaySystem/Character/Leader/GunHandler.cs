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
using Sources.Extension;

namespace Sources.GamePlaySystem.Leader
{
    public class GunModelView
    {
        public string GunId;
        public int DamageBulletCurrent;
        public ReactiveProperty<int> BulletTotal = new ReactiveProperty<int>(0);
        public ReactiveProperty<int> BulletAvailable = new ReactiveProperty<int>(0);
    }

    public class GunHandler
    {
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
            LoadGunInfoCurrent(LeaderKey.GunId_Default);
            CheckCanShoot();
        }

        private void LoadGunModels()
        {
            var gunDatas = _userProfile.LeaderDatas;

            foreach (var gunData in gunDatas)
            {
                var damageBullet = GetDamageBulletCurrent(gunData.Id, gunData.LevelUpgradeId);
                var gunModelView = new GunModelView
                {
                    GunId = gunData.Id,
                    DamageBulletCurrent = damageBullet
                };

                gunModelView.BulletTotal.Value = gunData.Quatity;

                var gunInfo = _leaderConfig.GetWeaponInfo(gunData.Id) as LeaderWeaponInfo;
                var bulletPerClip = (gunInfo.BulletsPerClip <= gunData.Quatity) ? gunInfo.BulletsPerClip : gunData.Quatity; 
                gunModelView.BulletAvailable.Value = bulletPerClip;

                GunModels.Add(gunData.Id, gunModelView);
            }
        }

        private void LoadGunInfoCurrent(string gunId)
        {
            if (GunModels.Remove(gunId, out GunModelView gunModel))
            {
                DamageBulletCurrent = gunModel.DamageBulletCurrent;
                GunModelCurrent.Value = gunModel;
            }
        }

        private int GetDamageBulletCurrent(string gunId, string levelUpgradeId)
        {
            var gunInfo = _leaderConfig.GetWeaponInfo(gunId);
            var damageInfo = gunInfo.GetLevelUpgradeInfo(levelUpgradeId);
            return damageInfo.DamageOrHp;
        }

        private void CheckCanShoot()
        {
            var bulletCurrent = GunModelCurrent.Value.BulletAvailable.Value;
            _isCanShoot = bulletCurrent > 0;
        }

        public void SubtractBullet()
        {
            if (GunModelCurrent.Value.BulletAvailable.Value <= 0) return;

            GunModelCurrent.Value.BulletAvailable.Value -= 1;
            IsShooting?.Invoke();

            CheckCanShoot();
        }

        public void ChangeGunModel(string gunId)
        {
            GunModels.Add(GunModelCurrent.Value.GunId, GunModelCurrent.Value);
            LoadGunInfoCurrent(gunId);
            CheckCanShoot();
        }

        public void AddBulletAvailable()
        {
            if (GunModelCurrent.Value.BulletTotal.Value > 0)
            {
                GunModelCurrent.Value.BulletAvailable.Value++;
                GunModelCurrent.Value.BulletTotal.Value--;
            }

            _userProfile.SubsctractQualityWeapon(GunModelCurrent.Value.GunId);
            CheckCanShoot();
        }

        public void Shooting()
        {
            if (!_isCanShoot) return;
            SubtractBullet();
        }
    }
}