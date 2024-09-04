using Cysharp.Threading.Tasks;
using Sources.DataBaseSystem;
using Sources.DataBaseSystem.Leader;
using Sources.SystemService;
using Sources.Utils.Singleton;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;

namespace Sources.GamePlaySystem.Leader
{
    public class GunModel
    {
        public string GunId;
        public string LevelDamage;
        public ReactiveProperty<int> BulletTotal = new ReactiveProperty<int>(0);
        public ReactiveProperty<int> BulletAvailable = new ReactiveProperty<int>(0);
    }

    public class InitLeaderSystemService : InitSystemService<LeaderSystem> { };

    public class LeaderSystem : BaseSystem
    {
        private const string _gunIdDefault = "gun-01";

        private DataBase _dataBase => Locator<DataBase>.Instance;
        private LeaderConfig _leaderConfig => _dataBase.GetConfig<LeaderConfig>();
        private GameData.GameData _gameData => Locator<GameData.GameData>.Instance;
        
        public ReactiveDictionary<string, GunModel> GunModels { get; private set; } = new ();
        public ReactiveProperty<string> GunIdCurrent { get; private set; } = new();
        public ReactiveProperty<bool> IsCanShoot { get; private set; } = new ();
        public Action Shooting;

        public override async UniTask Init()
        {
            LoadGunModels();
            LoadGunCurrent();
            CheckCanShoot();
        }

        private void LoadGunModels()
        {
            var gunModels = _gameData.SetDataLeaderData();
        
            foreach(var gunModel in gunModels)
            {
                var key = gunModel.GunId;
                GunModels.Add(key, gunModel);

                var gunInfo = _leaderConfig.GetWeaponInfo(key);
                GunModels[GunIdCurrent.Value].BulletAvailable.Value += gunInfo.BulletsPerClip;
                GunModels[GunIdCurrent.Value].BulletTotal.Value -= gunInfo.BulletsPerClip;
            }
        }

        private void LoadGunCurrent()
        {
            GunIdCurrent.Value = _gunIdDefault;
        }

        private void CheckCanShoot()
        {
            var bulletCurrent = GunModels[GunIdCurrent.Value].BulletAvailable.Value;
            IsCanShoot.Value = bulletCurrent > 0;
        }

        public void UpdateBullet()
        {
            GunModels[GunIdCurrent.Value].BulletAvailable.Value -= 1;
            CheckCanShoot();
            Shooting?.Invoke();
        }

        public void ChangeGunModel(string gunId)
        {
            GunIdCurrent.Value = gunId;
            CheckCanShoot();
        }

        public void AddBullet()
        {
            GunModels[GunIdCurrent.Value].BulletAvailable.Value += 1;
            GunModels[GunIdCurrent.Value].BulletTotal.Value -= 1;
            CheckCanShoot();
        }
    }
}