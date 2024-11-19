using Sources.DataBaseSystem;
using Sources.DataBaseSystem.Leader;
using Sources.Extension;
using Sources.GameData;
using Sources.GamePlaySystem.Character;
using Sources.Utils.Singleton;
using System;
using System.Collections.Generic;
using UniRx;

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
        private Dictionary<string, ReloadTimeHandlerBase> _reloadTimeHandlers = new();

        public Action IsShooting;
        public int DamageBulletCurrent { get; private set; }

        public ReactiveDictionary<string, GunModelView> GunModels { get; private set; } = new();
        public ReactiveProperty<GunModelView> GunModelCurrent { get; private set; } = new();
        public ReactiveProperty<float> TimeReloadCurrent { get; private set; } = new();

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

            if (!_reloadTimeHandlers.ContainsKey(gunId))
            {
                if (gunId == LeaderKey.GunId_04 || gunId == LeaderKey.GunId_05)
                {
                    var newReloadTimeHandler = new ReloadTimeGunMachineHandler();
                    newReloadTimeHandler.OnSetUp(gunModel);
                    _reloadTimeHandlers.Add(gunId, newReloadTimeHandler);
                }
                else
                {
                    var newReloadTimeHandler = new ReloadTimeGunNormalHandler();
                    newReloadTimeHandler.OnSetUp(gunModel);
                    _reloadTimeHandlers.Add(gunId, newReloadTimeHandler);
                }
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

            IsShooting?.Invoke();
            GunModelCurrent.Value.BulletAvailable.Value -= 1;

            _userProfile.SubsctractQualityWeapon(GunModelCurrent.Value.GunId);
            CheckCanShoot();
        }

        public void ChangeGunModel(string gunId)
        {
            GunModels.Add(GunModelCurrent.Value.GunId, GunModelCurrent.Value);
            LoadGunInfoCurrent(gunId);
            CheckCanShoot();
        }

        public void AddBulletAvailable(int bulletAdd)
        {
            if (GunModelCurrent.Value.BulletTotal.Value > 0)
            {
                GunModelCurrent.Value.BulletAvailable.Value += bulletAdd;
                GunModelCurrent.Value.BulletTotal.Value -= bulletAdd;
                GunModelCurrent.Value.BulletTotal.Value = Math.Max(0, GunModelCurrent.Value.BulletTotal.Value);
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