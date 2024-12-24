using Cysharp.Threading.Tasks;
using Sources.DataBaseSystem;
using Sources.Utils.Singleton;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace Sources.GamePlaySystem.MainGamePlay.Enemies
{
    public class ReloadTimeHandler
    {
        private DataBase _dataBase => Locator<DataBase>.Instance;
        private EnemiesConfig _enemiesConfig => _dataBase.GetConfig<EnemiesConfig>();

        private int _reloadTime;
        private string _enemyId;

        private IDisposable _disposable;

        public ReloadTimeHandler(ReactiveProperty<ActionType> actionType)
        {
            _disposable = actionType.Subscribe(value =>
            {
                if (value == ActionType.None) return;

                if (value == ActionType.Hit)
                    _reloadTime = _enemiesConfig.GetEnemyInfo(_enemyId).TimeToReloadHit;

                if (value == ActionType.Shoot)
                    _reloadTime = _enemiesConfig.GetEnemyInfo(_enemyId).TimeToReloadShoot;
            });
        }

        public void SetEnemyId(string id)
        {
            _enemyId = id;
        }

        public async UniTask ReloadingTime()
        {
            await UniTask.Delay(_reloadTime);
        }
    }
}