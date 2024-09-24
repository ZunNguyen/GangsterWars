using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sources.GamePlaySystem.MainGamePlay.Enemies
{
    public class EnemiesController
    {
        private List<EnemyHandler> _activeEnemyHandlers = new();
        public List<EnemyHandler> _availableEnemyHandlers { set; private get; } = new();

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
    }
}