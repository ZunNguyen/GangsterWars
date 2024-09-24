using Sirenix.OdinInspector;
using Sources.DataBaseSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Sources.DataBaseSystem
{
    [Serializable]
    public class EnemyInfo
    {
        public string Id;
        [PreviewField(100, ObjectFieldAlignment.Center)]
        public GameObject EnemyPrefab;
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