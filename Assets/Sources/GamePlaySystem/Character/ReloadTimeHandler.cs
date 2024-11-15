using Cysharp.Threading.Tasks;
using System;
using UniRx;
using UnityEngine;

namespace Sources.GamePlaySystem.Character
{
    public class ReloadTimeHandler
    {
        private float _timeReload;

        public ReactiveProperty<float> TimeReloadCurrent { get; private set; } = new(0);
        public Action<bool> IsReloading;

        public void OnSetUp(float timeReload)
        {
            _timeReload = timeReload;
        }

        public async void Reloading()
        {
            IsReloading?.Invoke(true);

            float endReloadTime = Time.time + _timeReload;
            TimeReloadCurrent.Value = _timeReload;

            while (TimeReloadCurrent.Value > 0)
            {
                TimeReloadCurrent.Value = (float)Math.Round(endReloadTime - Time.time, 1);

                if (TimeReloadCurrent.Value <= 0)
                {
                    TimeReloadCurrent.Value = 0;
                    break;
                }

                await UniTask.DelayFrame(1);
            }

            IsReloading?.Invoke(false);
        }
    }
}