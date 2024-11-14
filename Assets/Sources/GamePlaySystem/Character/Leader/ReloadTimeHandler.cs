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

        private int _maxBulletPerClip;
        private int _bulletCurrent;
        private float _onceTimeReload;
        private float _nextBulletReloadTime;
        private string _gunId;
        private bool _isCanReload;

        private GunHandler _gunHandler;
        private CancellationTokenSource _reloadCancellationTokenSource;
        private IDisposable _disposableGunModelCurrent;
        private IDisposable _disposableBulletAvailable;
        private IDisposable _disposableBulletTotal;

        public ReactiveProperty<float> TimeReloadCurrent { get; private set; } = new (0);

        public void OnSetUp(GunModelView gunModelView)
        {
            _gunId = gunModelView.GunId;
            _gunHandler = _leaderSystem.GunHandler;

            var weaponInfo = _leaderConfig.GetWeaponInfo(_gunId) as LeaderWeaponInfo;
            _maxBulletPerClip = weaponInfo.BulletsPerClip;
            _onceTimeReload = weaponInfo.ReloadTime;

            _disposableGunModelCurrent = _gunHandler.GunModelCurrent.Subscribe(value =>
            {
                _isCanReload = value.GunId == _gunId;
                _gunHandler.TimeReloadCurrent.Value = TimeReloadCurrent.Value;
                if (_isCanReload && TimeReloadCurrent.Value > 0)
                {
                    CountTimeToReLoad();
                }
            });

            _disposableBulletAvailable = gunModelView.BulletAvailable.Subscribe(value =>
            {
                _bulletCurrent = value;
                if (value != _maxBulletPerClip)
                {
                    CountTimeToReLoad();
                }
            });

            _disposableBulletTotal = gunModelView.BulletTotal.Subscribe(value =>
            {
                _isCanReload = value != 0;
            });

            _gunHandler.IsShooting += AddTimeReloadCurrent;
        }

        private void CountTimeToReLoad()
        {
            _reloadCancellationTokenSource?.Cancel();
            _reloadCancellationTokenSource = new CancellationTokenSource();

            CountTimeToReLoad(_reloadCancellationTokenSource.Token).Forget();
        }

        private async UniTask CountTimeToReLoad(CancellationToken cancellationToken)
        {
            float endReloadTime = Time.time + TimeReloadCurrent.Value;

            // 0.01f is offset when compare 2 float variable
            _nextBulletReloadTime = (_maxBulletPerClip - _bulletCurrent) * _onceTimeReload - _onceTimeReload + 0.01f;

            while (TimeReloadCurrent.Value > 0 && _isCanReload)
            {
                TimeReloadCurrent.Value = (float)Math.Round(endReloadTime - Time.time, 1);
                _gunHandler.TimeReloadCurrent.Value = TimeReloadCurrent.Value;

                if (TimeReloadCurrent.Value <= _nextBulletReloadTime)
                {
                    _gunHandler.AddBulletAvailable();
                }

                await UniTask.DelayFrame(1, cancellationToken : cancellationToken);
            }
        }

        private void AddTimeReloadCurrent()
        {
            if (_isCanReload) TimeReloadCurrent.Value += _onceTimeReload;
        }


        private void OnDisable()
        {
            _disposableBulletAvailable?.Dispose();
            _disposableBulletTotal?.Dispose();
            _disposableGunModelCurrent?.Dispose();
            _gunHandler.IsShooting -= AddTimeReloadCurrent;
        }
    }
}