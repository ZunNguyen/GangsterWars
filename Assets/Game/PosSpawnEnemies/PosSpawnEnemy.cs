using Cysharp.Threading.Tasks;
using Sources.GamePlaySystem.MainGamePlay;
using Sources.Utils.Singleton;
using UnityEngine;
using UniRx;
using Sources.DataBaseSystem;
using Sources.SpawnerSystem;
using System.Collections.Generic;
using Game.Character.Enemy.Abstract;
using Sources.GamePlaySystem.MainGamePlay.Enemies;
using Game.CanvasInGamePlay.Controller;

namespace Game.PosSpawnEnemies
{
    public class PosSpawnEnemy : MonoBehaviour
    {
        private const float _offsetDefaultZ = 0.1f;

        private DataBase _dataBase => Locator<DataBase>.Instance;
        private EnemiesConfig _enemiesConfig => _dataBase.GetConfig<EnemiesConfig>();

        private SpawnerManager _spawnerManager => Locator<SpawnerManager>.Instance;
        private MainGamePlaySystem _mainGamePlaySystem => Locator<MainGamePlaySystem>.Instance;

        private Dictionary<string, EnemyControllerAbstract> _enemiesCache = new Dictionary<string, EnemyControllerAbstract>();
        private int _indexPos;
        private Vector3 _offsetPos;

        [SerializeField] private CanvasInGamePlayController _canvasInGamePlayController;
        [SerializeField] private Transform _enemiesHolder;

        public void OnSetUp(int index)
        {
            _indexPos = index;
            _offsetPos = transform.position;

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
                var enemyController = enemyPrefab.GetComponent<EnemyControllerAbstract>();
                _enemiesCache.Add(enemyId, enemyController);

                Spawning(enemyController, enemyId);
                return;
            }

            var enemyTarget = _enemiesCache[enemyId];
            Spawning(enemyTarget, enemyId);
        }

        private void Spawning(EnemyControllerAbstract enemyController, string enemyId)
        {
            var enemyPrefab = _spawnerManager.Get(enemyController);
            _mainGamePlaySystem.SpawnEnemiesHandler.AddEnemyToList(enemyPrefab);
            enemyPrefab.OnSetUp(_canvasInGamePlayController, enemyId);
            enemyPrefab.transform.SetParent(_enemiesHolder);

            _offsetPos.z -= _offsetDefaultZ;
            enemyPrefab.transform.position = _offsetPos;
        }
    }
}