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

        private async void OnBulletAvailableChanged(int bulletCurrent)
        {
            _reloadCancellationTokenSource?.Cancel();
            _reloadCancellationTokenSource = new CancellationTokenSource();
            CountTimeToReLoad(bulletCurrent, _reloadCancellationTokenSource.Token).Forget();
        }

        private async UniTask CountTimeToReLoad(int bulletCurrent, CancellationToken cancellationToken)
        {
            await UniTask.Delay(2000, cancellationToken : cancellationToken);
            while (bulletCurrent < _maxBulletPerClip)
            {
                TimeReloadCurrent.Value += Time.deltaTime;
                if (Math.Abs(TimeReloadCurrent.Value - _timeToReloadOneBullet) < 0.01f)
                {
                    _gunHandler.AddBullet();
                    ResetTimeReloadCurrent();
                    bulletCurrent += 1;
                }

                await UniTask.DelayFrame(1, cancellationToken: cancellationToken);
            }
        }

        private void ResetTimeReloadCurrent()
        {
            TimeReloadCurrent.Value = 0; 
        }
    }
}

