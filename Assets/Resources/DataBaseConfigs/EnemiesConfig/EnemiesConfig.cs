using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Sources.DataBaseSystem
{
    [Serializable]
    public class EnemyInfo
    {
        public string Id;
        [PreviewField(100, ObjectFieldAlignment.Center)]
        public GameObject EnemyPrefab;
        public List<WaveEnemy> WaveEnemies;
        public Dictionary<string, WaveEnemy> WaveEnemyCache { get; private set; } = new();

        public WaveEnemy GetWaveEnemy(string id)
        {
            if (!WaveEnemyCache.ContainsKey(id))
            {
                var waveEnemy = WaveEnemies.Find(x => x.Id == id);
                WaveEnemyCache.Add(id, waveEnemy);
            }
            return WaveEnemyCache[id];
        }
    }

    [Serializable]
    public class WaveEnemy
    {
        public string Id;
        public int Damage;
        public int Hp;

        [GD.MinMaxSlider.MinMaxSlider(0, 10000)]
        public Vector2Int coinReward;

        [ProgressBar(0, 100)]
        public int PercentChance;
    }

    public class EnemiesConfig : DataBaseConfig
    {
        [SerializeField] private List<EnemyInfo> _enemies;
        private Dictionary<string, EnemyInfo> _enemyInfoCache = new();

        public EnemyInfo GetEnemyInfo(string id)
        {
            if (!_enemyInfoCache.ContainsKey(id))
            {
                var newEnemyInfo = _enemies.Find(x => x.Id == id);
                _enemyInfoCache.Add(id, newEnemyInfo);
            }

            return _enemyInfoCache[id];
        }
    }
}