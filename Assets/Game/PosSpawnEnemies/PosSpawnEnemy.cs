using Cysharp.Threading.Tasks;
using Sources.GamePlaySystem.MainGamePlay;
using Sources.Utils.Singleton;
using UnityEngine;
using UniRx;
using Sources.DataBaseSystem;
using Sources.SpawnerSystem;
using System.Collections.Generic;
using Game.Character.Enemy;
using Sources.GamePlaySystem.MainGamePlay.Enemies;
using Game.CanvasInGamePlay.Controller;

namespace Game.PosSpawnEnemies
{
    public class PosSpawnEnemy : MonoBehaviour
    {
        private DataBase _dataBase => Locator<DataBase>.Instance;
        private EnemiesConfig _enemiesConfig => _dataBase.GetConfig<EnemiesConfig>();

        private SpawnerManager _spawnerManager => Locator<SpawnerManager>.Instance;
        private MainGamePlaySystem _mainGamePlaySystem => Locator<MainGamePlaySystem>.Instance;

        private Dictionary<string, EnemyController> _enemiesCache = new Dictionary<string, EnemyController>();
        private int _indexPos;

        [SerializeField] private CanvasInGamePlayController _canvasInGamePlayController;

        public void OnSetUp(int index)
        {
            _indexPos = index;

            _mainGamePlaySystem.SpawnEnemiesHandler.EnemyModel.Subscribe(value =>
            {
                if (value == null) return;
                SpawnEnemy(value);
            }).AddTo(this);
        }

        private void SpawnEnemy(Enemy enemy)
        {
            if (!enemy.IndexPos.Contains(_indexPos)) return;

            var enemyId = enemy.EnemyId;

            if (!_enemiesCache.ContainsKey(enemyId))
            {
                var enemyPrefab = _enemiesConfig.GetEnemyInfo(enemyId).EnemyPrefab;
                var enemyController = enemyPrefab.GetComponent<EnemyController>();
                _enemiesCache.Add(enemyId, enemyController);

                Spawning(enemyController, enemyId);
                return;
            }

            var enemyTarget = _enemiesCache[enemyId];
            Spawning(enemyTarget, enemyId);
        }

        private void Spawning(EnemyController enemyController, string enemyId)
        {
            var enemyPrefab = _spawnerManager.Get(enemyController);
            _mainGamePlaySystem.SpawnEnemiesHandler.AddEnemyToList(enemyPrefab);
            enemyPrefab.transform.position = this.transform.position;
            enemyPrefab.OnSetUp(_canvasInGamePlayController, enemyId);
        }
    }
}