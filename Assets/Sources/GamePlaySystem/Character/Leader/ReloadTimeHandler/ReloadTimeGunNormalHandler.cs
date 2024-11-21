using BestHTTP.SecureProtocol.Org.BouncyCastle.Math.Field;
using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UniRx;
using UnityEngine;

namespace Sources.GamePlaySystem.Leader
{
    public class ReloadTimeGunNormalHandler : ReloadTimeHandlerBase
    {
        private const int _bulletAdd = 1;

        private int _bulletCurrent;
        private float _nextBulletReloadTime;
        private CancellationTokenSource _reloadCancellationTokenSource;
        private IDisposable _disposableBulletAvailable;

        public override void OnSetUp(GunModelView gunModelView)
        {
            base.OnSetUp(gunModelView);
            _gunHandler.IsShooting += AddTimeReloadCurrent;
            _gunHandler.EmptyBullet += EmptyBullet;
        }

        protected override void SubscribeBulletAvailable()
        {
            _disposableBulletAvailable = _gunModelView.BulletAvailable.Subscribe(value =>
            {
                _bulletCurrent = value;
                if (value != _maxBulletPerClip) CountTimeToReLoad();
            });
        }

        override protected void SubscribeBulletTotal()
        {
            _disposableBulletTotal = _gunModelView.BulletTotal.Subscribe(value =>
            {
                _isCanReload = value != 0;
            });
        }

        private void AddTimeReloadCurrent()
        {
            if (_isCanReload) TimeReloadCurrent.Value += _onceTimeReload;
        }

        private async void EmptyBullet()
        {
            _isCanReload = false;
            await UniTask.DelayFrame(2);
            TimeReloadCurrent.Value = 0;
            _gunHandler.TimeReloadCurrent.Value = 0;
        }

        protected override void CountTimeToReLoad()
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
                    _gunHandler.AddBulletAvailable(_bulletAdd);
                }

                await UniTask.DelayFrame(1, cancellationToken : cancellationToken);
            }
        }
        
        protected override void OnDisable()
        {
            base.OnDisable();
            _disposableBulletAvailable?.Dispose();
            _gunHandler.IsShooting -= AddTimeReloadCurrent;
            _gunHandler.EmptyBullet -= EmptyBullet;
        }
    }
}