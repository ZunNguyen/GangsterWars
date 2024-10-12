using Cysharp.Threading.Tasks;
using Sources.DataBaseSystem;
using Sources.DataBaseSystem.Leader;
using Sources.Utils.Singleton;
using System;
using System.Collections.Generic;
using System.Threading;
using UniRx;
using Unity.Mathematics;
using UnityEngine;

namespace Sources.GamePlaySystem.Leader
{
    public class ReloadTimeInfo
    {
        public float SaveTimeReloadCurrent = 0;
        public float SaveReloadDuration = 0;
    }

    public class ReloadTimeHandler
    {
        private const string _gunIdCurrentDefault = "";

        private DataBase _dataBase => Locator<DataBase>.Instance;
        private LeaderConfig _leaderConfig => _dataBase.GetConfig<LeaderConfig>();

        private LeaderSystem _leaderSystem => Locator<LeaderSystem>.Instance;
        private GunHandler _gunHandler;

        private IDisposable _currentSubscription;
        private int _maxBulletPerClip;
        private float _timeToReloadOneBullet;
        private CancellationTokenSource _reloadCancellationTokenSource;
        private Dictionary<string, ReloadTimeInfo> _reloadTimeInfos = new Dictionary<string, ReloadTimeInfo>();

        public ReactiveProperty<float> TimeReloadCurrent { get; private set; } = new (0);

        private string _gunIdCurrent = "";
        private float _startTime;
        private float _reloadDuration = 0;
        private float _nextBulletReloadTime;

        public void OnSetUp()
        {
            _gunHandler = _leaderSystem.GunHandler;

            _gunHandler.GunModelCurrent.Subscribe(OnGunModelChanged);

            _gunHandler.IsShooting += AddTimeReloadCurrent;
        }

        private void OnGunModelChanged(GunModel gunModel)
        {
            SaveTimeReloadCurrent();

            _gunIdCurrent = gunModel.GunId;
            _currentSubscription?.Dispose();
            UpdateReloadInfo();
        }

        private void UpdateReloadInfo()
        {
            _maxBulletPerClip = _leaderConfig.WeaponInfoCache[_gunIdCurrent].BulletsPerClip;
            _timeToReloadOneBullet = _leaderConfig.WeaponInfoCache[_gunIdCurrent].ReloadTime;
            _currentSubscription = _gunHandler.GunModelCurrent.Value.BulletAvailable.Subscribe(OnBulletAvailableChanged);
        }

        private void OnBulletAvailableChanged(int bulletAvailableCurrent)
        {
            _startTime = Time.realtimeSinceStartup;
            _reloadDuration = (_maxBulletPerClip - bulletAvailableCurrent) * _timeToReloadOneBullet;
            _nextBulletReloadTime = Math.Max(0, _reloadDuration - _timeToReloadOneBullet);

            LoadTimeReloadCurrent(bulletAvailableCurrent);

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

                if (TimeReloadCurrent.Value <= _nextBulletReloadTime)
                {
                    _gunHandler.AddBulletAvailable();
                    bulletCurrent += 1;
                }

                await UniTask.DelayFrame(1, cancellationToken: cancellationToken);
            }
        }

        private void SaveTimeReloadCurrent()
        {
            if (_gunIdCurrent == _gunIdCurrentDefault) return;

            if (!_reloadTimeInfos.ContainsKey(_gunIdCurrent))
            {
                var newReloadTimeInfo = new ReloadTimeInfo();
                _reloadTimeInfos.Add(_gunIdCurrent, newReloadTimeInfo);
            }

            _reloadTimeInfos[_gunIdCurrent].SaveTimeReloadCurrent = TimeReloadCurrent.Value;
            _reloadTimeInfos[_gunIdCurrent].SaveReloadDuration = _reloadDuration;
        }

        private void LoadTimeReloadCurrent(int bulletCurrent)
        {
            if (!_reloadTimeInfos.ContainsKey(_gunIdCurrent))
            {
                TimeReloadCurrent.Value = 0;
                return;
            }

            if (bulletCurrent != _maxBulletPerClip)
            {
                TimeReloadCurrent.Value = _reloadTimeInfos[_gunIdCurrent].SaveTimeReloadCurrent;
                return;
            }
            
            if (_reloadTimeInfos[_gunIdCurrent].SaveReloadDuration == 0) return;
            
            if (_reloadTimeInfos[_gunIdCurrent].SaveReloadDuration < _reloadDuration)
            {
                _reloadDuration = _reloadTimeInfos[_gunIdCurrent].SaveReloadDuration;
                return;
            }
        }

        private void AddTimeReloadCurrent()
        {
            TimeReloadCurrent.Value += _timeToReloadOneBullet;
        }
    }
}