using Cysharp.Threading.Tasks;
using Sources.DataBaseSystem;
using Sources.GamePlaySystem.Leader;
using Sources.Utils.Singleton;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UniRx;
using UnityEngine;

namespace Sources.GamePlaySystem.Bomber
{
    public class ReloadTimeHandler
    {
        private DataBase _dataBase => Locator<DataBase>.Instance;
        private BomberConfig _bomberConfig => _dataBase.GetConfig<BomberConfig>();

        private float _timeReload;

        public ReactiveProperty<float> TimeReloadCurrent { get; private set; } = new(0);
        public Action CompleteReload;

        public void OnSetUp()
        {
            _timeReload = 5;
        }

        public async void Reloading()
        {
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

            CompleteReload?.Invoke();
        }
    }
}