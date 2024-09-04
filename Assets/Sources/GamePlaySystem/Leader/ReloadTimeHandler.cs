using Cysharp.Threading.Tasks;
using Sources.DataBaseSystem;
using Sources.DataBaseSystem.Leader;
using Sources.Utils.Singleton;
using System;
using UniRx;
using UnityEngine;

namespace Sources.GamePlaySystem.Leader
{
    public class ReloadTimeHandler
    {
        private DataBase _dataBase => Locator<DataBase>.Instance;
        private LeaderConfig _leaderConfig => _dataBase.GetConfig<LeaderConfig>();

        private LeaderSystem _leaderSystem => Locator<LeaderSystem>.Instance;
        private GunHandler _gunHandler;

        private IDisposable _currentSubscription;
        private int _maxBulletPerClip;
        private float _timeToReloadOneBullet;

        public ReactiveProperty<float> TimeReloadCurrent = new ();

        public void OnSetUp()
        {
            _gunHandler = _leaderSystem.GunHandler;

            _gunHandler.GunModelCurrent.Subscribe(OnGunModelChanged);

            _gunHandler.Shooting += ResetTimeReloadCurrent;
        }

        private void OnGunModelChanged(GunModel gunModel)
        {
            _currentSubscription?.Dispose();
            UpdateReloadInfo(gunModel.GunId);
        }

        private void UpdateReloadInfo(string gunId)
        {
            _maxBulletPerClip = _leaderConfig.WeaponInfoCache[gunId].BulletsPerClip;
            _timeToReloadOneBullet = _leaderConfig.WeaponInfoCache[gunId].ReloadTime;
            _currentSubscription = _gunHandler.GunModelCurrent.Value.BulletAvailable.Subscribe(OnBulletAvailableChanged);
        }

        private void OnBulletAvailableChanged(int bulletCurrent)
        {
            CountTimeToReLoad(bulletCurrent);
        }

        private async void CountTimeToReLoad(int bulletCurrent)
        {
            var bullet = bulletCurrent;
            while (bullet < _maxBulletPerClip)
            {
                TimeReloadCurrent.Value += Time.deltaTime;
                if (Math.Abs(TimeReloadCurrent.Value - _timeToReloadOneBullet) < 0.1f)
                {
                    _gunHandler.AddBullet();
                    ResetTimeReloadCurrent();
                    bullet += 1;
                }

                await UniTask.DelayFrame(1);
            }
        }

        private void ResetTimeReloadCurrent()
        {
            TimeReloadCurrent.Value = 0; 
        }
    }
}

