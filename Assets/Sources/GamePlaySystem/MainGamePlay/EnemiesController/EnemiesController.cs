using Sources.DataBaseSystem;
using Sources.Utils.Singleton;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace Sources.GamePlaySystem.MainGamePlay.Enemies
{
    public class EnemiesController
    {
        private DataBase _dataBase => Locator<DataBase>.Instance;
        private SpawnWaveConfig _spawnWaveConfig => _dataBase.GetConfig<SpawnWaveConfig>();

        private List<EnemyHandler> _activeEnemyHandlers = new();
        public List<EnemyHandler> _availableEnemyHandlers { set; private get; } = new();

        public int TotalHpEnemies { get; private set; }
        public ReactiveProperty<int> HpEnemiesCurrent { get; private set; } = new();

        public void OnSetUp(string waveId)
        {
            TotalHpEnemies = HpEnemiesCurrent.Value = _spawnWaveConfig.GetWaveInfo(waveId).TotalHp;
        }

        public EnemyHandler GetAvailableEnemyHandler()
        {
            if (_availableEnemyHandlers.Count == 0)
            {
                var newEnemyHandler = new EnemyHandler();
                _availableEnemyHandlers.Add(newEnemyHandler);
            }

            var enemyHandler = _availableEnemyHandlers[0];
            MoveToActiveList(enemyHandler);
            return enemyHandler;
        }

        public void UnActiveEnemyHandler(EnemyHandler enemyHandler)
        {
            MoveToAvailableList(enemyHandler);
        }

        private void MoveToAvailableList(EnemyHandler enemyHandler)
        {
            _availableEnemyHandlers.Add(enemyHandler);
            _activeEnemyHandlers.Remove(enemyHandler);
        }

        private void MoveToActiveList(EnemyHandler enemyHandler)
        {
            _activeEnemyHandlers.Add(enemyHandler);
            _availableEnemyHandlers.Remove(enemyHandler);
        }

        public void SubstractHpTotal(int damge)
        {
            HpEnemiesCurrent.Value -= damge;
        }
    }
}