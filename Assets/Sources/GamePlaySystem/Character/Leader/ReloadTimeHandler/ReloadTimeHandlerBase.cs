using Sources.DataBaseSystem;
using Sources.DataBaseSystem.Leader;
using Sources.GamePlaySystem.GameResult;
using Sources.Utils.Singleton;
using System;
using UniRx;
using Unity.VisualScripting;
using UnityEngine;

namespace Sources.GamePlaySystem.Leader
{
    public abstract class ReloadTimeHandlerBase
    {
        private DataBase _dataBase => Locator<DataBase>.Instance;
        private LeaderConfig _leaderConfig => _dataBase.GetConfig<LeaderConfig>();
        private LeaderSystem _leaderSystem => Locator<LeaderSystem>.Instance;
        private GameResultSystem _gameResultSystem => Locator<GameResultSystem>.Instance;

        private string _gunId;
        private IDisposable _disposableGunModelCurrent;
        
        protected int _maxBulletPerClip;
        protected float _onceTimeReload;
        protected bool _isCanReload;
        protected bool _isEndGame;
        protected GunModelView _gunModelView;
        protected GunHandler _gunHandler;
        protected IDisposable _disposableBulletTotal;

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
            SubscribeEndGame();
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

        private void SubscribeEndGame()
        {
            _gameResultSystem.IsEndGame += SetEndGame;
        }

        private void SetEndGame(bool isEndGame)
        {
            _isEndGame = isEndGame;
        }

        protected abstract void SubscribeBulletTotal();

        protected abstract void SubscribeBulletAvailable();

        protected abstract void CountTimeToReLoad();

        protected virtual void OnDisable()
        {
            _disposableBulletTotal?.Dispose();
            _disposableGunModelCurrent?.Dispose();
            _gameResultSystem.IsEndGame -= SetEndGame;
        }

        private void OnDestroy()
        {
            OnDisable();
        }
    }
}