using Cysharp.Threading.Tasks;
using Sources.DataBaseSystem;
using Sources.DataBaseSystem.Leader;
using Sources.Utils.Singleton;
using System;
using System.Threading;
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
        private CancellationTokenSource _reloadCancellationTokenSource;

        public ReactiveProperty<float> TimeReloadCurrent { get; private set; } = new ();

        private float startTime;
        private float reloadDuration;
        private float nextBulletReloadTime;

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
            startTime = Time.realtimeSinceStartup;
            reloadDuration = (_maxBulletPerClip - bulletCurrent) * _timeToReloadOneBullet;
            nextBulletReloadTime = startTime + _timeToReloadOneBullet;

            _reloadCancellationTokenSource?.Cancel();
            _reloadCancellationTokenSource = new CancellationTokenSource();
            CountTimeToReLoad(bulletCurrent, _reloadCancellationTokenSource.Token).Forget();
        }

        private async UniTask CountTimeToReLoad(int bulletCurrent, CancellationToken cancellationToken)
        {
            while (bulletCurrent < _maxBulletPerClip)
            {
                float elapsedTime = Time.realtimeSinceStartup - startTime;
                TimeReloadCurrent.Value = reloadDuration - elapsedTime;
                if (Time.realtimeSinceStartup >= nextBulletReloadTime)
                {
                    _gunHandler.AddBullet();
                    bulletCurrent += 1;
                    nextBulletReloadTime += _timeToReloadOneBullet;
                }

                await UniTask.DelayFrame(1, cancellationToken: cancellationToken);
            }

            ResetTimeReloadCurrent();
        }

        private void ResetTimeReloadCurrent()
        {
            TimeReloadCurrent.Value = 0;
        }
    }
}