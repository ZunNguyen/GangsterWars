using Cysharp.Threading.Tasks;
using Sources.DataBaseSystem;
using Sources.DataBaseSystem.Leader;
using Sources.Utils.Singleton;
using System;
using UniRx;
using UnityEngine;

namespace Sources.GamePlaySystem.Leader
{
    public class ReloadTimeController
    {
        private DataBase _dataBase => Locator<DataBase>.Instance;
        private LeaderConfig _leaderConfig => _dataBase.GetConfig<LeaderConfig>();

        private LeaderSystem _leaderSystem => Locator<LeaderSystem>.Instance;

        private IDisposable _currentSubscription;
        private int _maxBulletPerClip;
        private int _timeToReloadOneBullet;
        private float _timeReloadCurrent;

        public void OnSetUp()
        {
            _leaderSystem.GunIdCurrent.Subscribe(value =>
            {
                _currentSubscription?.Dispose();
                _maxBulletPerClip = _leaderConfig.WeaponInfoCache[value].BulletsPerClip;
                _timeToReloadOneBullet = _leaderConfig.WeaponInfoCache[value].ReloadTime;

                _currentSubscription = _leaderSystem.GunModels[value].BulletAvailable.Subscribe(value =>
                {
                    CountTimeToReLoad(value);
                });
            });

            _leaderSystem.Shooting += ResetTimeReloadCurrent;
        }

        private async void CountTimeToReLoad(int bulletCurrent)
        {
            await UniTask.Delay(1000);
            while (bulletCurrent < _maxBulletPerClip)
            {
                _timeReloadCurrent += Time.deltaTime;
                if (Math.Abs(_timeReloadCurrent - _timeToReloadOneBullet) < 0.01f)
                {
                    _leaderSystem.AddBullet();
                }
            }
        }

        private void ResetTimeReloadCurrent()
        {
            _timeReloadCurrent = 0; 
        }
    }
}

