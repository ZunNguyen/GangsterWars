using Sources.FTUE.System;
using Sources.Utils.Singleton;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sources.FTUE.GameObject
{
    public class FTUESpawnEnemy : MonoBehaviour
    {
        private FTUESystem _ftueSystem => Locator<FTUESystem>.Instance;

        [SerializeField] private UnityEngine.GameObject _enemyPrefab;

        private void Awake()
        {
            _ftueSystem.SpawnEnemyFTUE += SpawnEnemy;
        }

        private void SpawnEnemy()
        {
            var enemy = Instantiate(_enemyPrefab, transform);
        }
    }
}