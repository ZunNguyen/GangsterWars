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
using Sources.GamePlaySystem.GameResult;
using DG.Tweening;

namespace Game.PosSpawnEnemies
{
    public class PosSpawnEnemy : MonoBehaviour
    {
#if UNITY_ANDROID
        private const float _posXSpawnEnemy = 13.5f;
#else
        private const float _posXSpawnEnemy = 11f;
#endif
        private const float _offsetDefaultZ = 0.01f;

        private DataBase _dataBase => Locator<DataBase>.Instance;
        private EnemiesConfig _enemiesConfig => _dataBase.GetConfig<EnemiesConfig>();

        private SpawnerManager _spawnerManager => Locator<SpawnerManager>.Instance;
        private MainGamePlaySystem _mainGamePlaySystem => Locator<MainGamePlaySystem>.Instance;
        private GameResultSystem _gameResultSystem => Locator<GameResultSystem>.Instance;

        private bool _isEndGame = false;
        private int _indexPos;
        private Vector3 _offsetPos;
        private Dictionary<string, EnemyControllerAbstract> _enemiesCache = new Dictionary<string, EnemyControllerAbstract>();

        [SerializeField] private Transform _enemiesHolder;

        public void OnSetUp(int index)
        {
            SetPosX();
            _indexPos = index;
            _offsetPos = transform.position;

            _mainGamePlaySystem.SpawnEnemiesHandler.EnemyModel.Subscribe(value =>
            {
                if (value == null) return;
                SpawnEnemy(value);
            }).AddTo(this);

            _gameResultSystem.IsUserWin += EndGame;
        }

        private void SetPosX()
        {
            var posEnemy = gameObject.transform.position;
            posEnemy.x = _posXSpawnEnemy;
            transform.position = posEnemy;
        }

        private void EndGame(bool isEndGame)
        {
            _isEndGame = true;
        }

        private void SpawnEnemy(Enemy enemy)
        {
            if (!enemy.IndexPos.Contains(_indexPos) || _isEndGame) return;

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
            var newEnemy = _spawnerManager.Get(enemyController);
            _mainGamePlaySystem.SpawnEnemiesHandler.AddEnemyToList(newEnemy);
            newEnemy.OnSetUp(enemyId, _indexPos);
            newEnemy.transform.SetParent(_enemiesHolder, false);

            _offsetPos.z -= _offsetDefaultZ;
            newEnemy.transform.position = _offsetPos;
        }
    }
}