using Cysharp.Threading.Tasks;
using Sources.DataBaseSystem;
using Sources.DataBaseSystem.Leader;
using Sources.GamePlaySystem.Leader;
using Sources.Utils.Singleton;
using System;
using UniRx;
using UnityEngine;

namespace Sources.GamePlaySystem.Character
{
    public class ReloadTimeGunMachineHandler : ReloadTimeHandlerBase
    {
        private IDisposable _disposableBulletAvailable;

        protected override void SubscribeBulletAvailable()
        {
            _disposableBulletAvailable = _gunModelView.BulletAvailable.Subscribe(value =>
            {
                if (value == 0)
                {
                    TimeReloadCurrent.Value = _onceTimeReload;
                    CountTimeToReLoad();
                }
            });
        }

        protected override async void CountTimeToReLoad()
        {
            float endReloadTime = Time.time + TimeReloadCurrent.Value;

            while (TimeReloadCurrent.Value > 0 && _isCanReload)
            {
                TimeReloadCurrent.Value = (float)Math.Round(endReloadTime - Time.time, 1);
                _gunHandler.TimeReloadCurrent.Value = TimeReloadCurrent.Value;

                if (TimeReloadCurrent.Value <= 0)
                {
                    TimeReloadCurrent.Value = Math.Max(0, TimeReloadCurrent.Value);
                }

                await UniTask.DelayFrame(1);
            }

            if (_isCanReload && TimeReloadCurrent.Value == 0)
            {
                _gunHandler.AddBulletAvailable(_maxBulletPerClip);
            }
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _disposableBulletAvailable?.Dispose();
        }
    }
}