using Cysharp.Threading.Tasks;
using Sources.DataBaseSystem;
using Sources.DataBaseSystem.Leader;
using Sources.Utils.Singleton;
using System;
using System.Threading;
using UniRx;
using Unity.Mathematics;
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
        private CancellationTokenSource _reloadCancellationTokenSource;

        public ReactiveProperty<float> TimeReloadCurrent { get; private set; } = new ();

        private float _startTime;
        private float _reloadDuration;
        private float _nextBulletReloadTime;

        public void OnSetUp()
        {
            _gunHandler = _leaderSystem.GunHandler;

            _gunHandler.GunModelCurrent.Subscribe(OnGunModelChanged);

            _gunHandler.IsShooting += AddTimeReloadCurrent;
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

        private void OnBulletAvailableChanged(int bulletAvailableCurrent)
        {
            _startTime = Time.realtimeSinceStartup;
            _reloadDuration = (_maxBulletPerClip - bulletAvailableCurrent) * _timeToReloadOneBullet;
            _nextBulletReloadTime = _startTime + _timeToReloadOneBullet;

            _reloadCancellationTokenSource?.Cancel();
            _reloadCancellationTokenSource = new CancellationTokenSource();
            CountTimeToReLoad(bulletAvailableCurrent, _reloadCancellationTokenSource.Token).Forget();
        }

        private async UniTask CountTimeToReLoad(int bulletCurrent, CancellationToken cancellationToken)
        {
            while (bulletCurrent < _maxBulletPerClip)
            {
                float elapsedTime = Time.realtimeSinceStartup - _startTime;
                TimeReloadCurrent.Value = (float)Math.Round(_reloadDuration - elapsedTime, 1);

                if (Time.realtimeSinceStartup >= _nextBulletReloadTime)
                {
                    _gunHandler.AddBulletAvailable();
                    bulletCurrent += 1;
                }

                await UniTask.DelayFrame(1, cancellationToken: cancellationToken);
            }
        }

        private void AddTimeReloadCurrent()
        {
            TimeReloadCurrent.Value += _timeToReloadOneBullet;
        }
    }
}