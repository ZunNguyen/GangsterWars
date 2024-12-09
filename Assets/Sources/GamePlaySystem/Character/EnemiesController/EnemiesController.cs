using Sources.DataBaseSystem;
using Sources.Utils.Singleton;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace Sources.GamePlaySystem.MainGamePlay.Enemies
{
    public class EnemiesController
    {
        private DataBase _dataBase => Locator<DataBase>.Instance;
        private WavesConfig _spawnWaveConfig => _dataBase.GetConfig<WavesConfig>();

        private List<EnemyHandler> _activeEnemyHandlers = new();
        public List<EnemyHandler> _availableEnemyHandlers { set; private get; } = new();

        public int TotalHpEnemies { get; private set; }
        public List<Transform> ShieldPlayerPos { get; private set; } = new();
        public ReactiveProperty<int> HpEnemiesCurrent { get; private set; } = new();

        public void OnSetUp(string waveId)
        {
            TotalHpEnemies = HpEnemiesCurrent.Value = _spawnWaveConfig.GetSpawnWaveInfo(waveId).TotalHp;
            ShieldPlayerPos.Clear();
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

        public void MoveToAvailableList(EnemyHandler enemyHandler)
        {
            var index = _activeEnemyHandlers.IndexOf(enemyHandler);
            var enemyHandlerCache = _activeEnemyHandlers[index];

            _availableEnemyHandlers.Add(enemyHandlerCache);
            _activeEnemyHandlers.Remove(enemyHandlerCache);
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

        public void SetShieldPlayerPos(Transform pos)
        {
            ShieldPlayerPos.Add(pos);
        }
    }
}