using Sources.DataBaseSystem;
using Sources.DataBaseSystem.Leader;
using Sources.Utils.Singleton;
using System;
using UniRx;
using UnityEngine;

namespace Sources.GamePlaySystem.Leader
{
    public abstract class ReloadTimeHandlerBase
    {
        private DataBase _dataBase => Locator<DataBase>.Instance;
        private LeaderConfig _leaderConfig => _dataBase.GetConfig<LeaderConfig>();
        private LeaderSystem _leaderSystem => Locator<LeaderSystem>.Instance;

        private string _gunId;
        private IDisposable _disposableGunModelCurrent;
        private IDisposable _disposableBulletTotal;

        protected int _maxBulletPerClip;
        protected float _onceTimeReload;
        protected bool _isCanReload;
        protected GunModelView _gunModelView;
        protected GunHandler _gunHandler;

        public ReactiveProperty<float> TimeReloadCurrent { get; private set; } = new (0);

        public virtual void OnSetUp(GunModelView gunModelView)
        {
            _gunId = gunModelView.GunId;
            _gunHandler = _leaderSystem.GunHandler;
            _gunModelView = gunModelView;

            var weaponInfo = _leaderConfig.GetWeaponInfo(_gunId) as LeaderWeaponInfo;
            _maxBulletPerClip = weaponInfo.BulletsPerClip;
            _onceTimeReload = weaponInfo.ReloadTime;

            SubscribeGunModelCurrent();
            SubscribeBulletAvailable();
            SubscribeBulletTotal();
        }

        private void SubscribeGunModelCurrent()
        {
            _disposableGunModelCurrent = _gunHandler.GunModelCurrent.Subscribe(value =>
            {
                _isCanReload = value.GunId == _gunId;
                if (_isCanReload) _gunHandler.TimeReloadCurrent.Value = TimeReloadCurrent.Value;
                if (_isCanReload && TimeReloadCurrent.Value > 0) CountTimeToReLoad();
            });
        }

        private void SubscribeBulletTotal()
        {
            _disposableBulletTotal = _gunModelView.BulletTotal.Subscribe(value =>
            {
                _isCanReload = value != 0;
            });
        }

        protected virtual void OnDisable()
        {
            _disposableBulletTotal?.Dispose();
            _disposableGunModelCurrent?.Dispose();
        }

        protected abstract void SubscribeBulletAvailable();

        protected abstract void CountTimeToReLoad();
    }
}