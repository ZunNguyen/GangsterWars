using Resources.CSV;
using Sirenix.OdinInspector;
using Sources.Utils.String;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Sources.DataBaseSystem
{
    [Serializable]
    public class EnemyInfo : IReadCSVData
    {
        public string Id;
        [PreviewField(100, ObjectFieldAlignment.Center)]
        public GameObject EnemyPrefab;
        public List<WaveEnemy> WaveEnemies = new();
        public Dictionary<string, WaveEnemy> WaveEnemyCache { get; private set; } = new();

        public TextAsset CSVFile;

        public WaveEnemy GetWaveEnemy(string id)
        {
            if (!WaveEnemyCache.ContainsKey(id))
            {
                var waveEnemy = WaveEnemies.Find(x => x.Id == id);
                WaveEnemyCache.Add(id, waveEnemy);
            }
            return WaveEnemyCache[id];
        }

#if UNITY_EDITOR
        private const int _startRowIndex = 1;
        private const int _startIndexColWaveId = 0;
        private const int _startIndexColDamage = 1;
        private const int _startIndexColHp = 2;
        private const int _startIndexColCoinReward = 3;
        private const int _startIndexColPerChance = 4;

        private string[] _datas;
        private int _rowCount;
        private int _columnCount;
        [Button]
        public void ReadFile()
        {
            WaveEnemies.Clear();
            _datas = CSVFile.text.Split(new string[] { ",", "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries);
            string[] lines = CSVFile.text.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);

            _rowCount = lines.Length;
            _columnCount = _datas.Length / _rowCount;

            for (int row = _startRowIndex; row < _rowCount; row++)
            {
                var indexId = row * _columnCount + _startIndexColWaveId;
                var indexDamage = row * _columnCount + _startIndexColDamage;
                var indexHp = row * _columnCount + _startIndexColHp;
                var indexCoinReward = row * _columnCount + _startIndexColCoinReward;
                var indexPerChance = row * _columnCount + _startIndexColPerChance;

                var coinReward = StringUtils.SeparateString_2(_datas[indexCoinReward]);
                var coinRewardMin = int.Parse(coinReward[0]);
                var coinRewardMax = int.Parse(coinReward[1]);

                var waveEnemy = new WaveEnemy
                {
                    Id = _datas[indexId],
                    Damage = int.Parse(_datas[indexDamage]),
                    Hp = int.Parse(_datas[indexHp]),
                    coinReward = new Vector2Int(coinRewardMin, coinRewardMax),
                    PercentChance = int.Parse(_datas[indexPerChance])
                };

                WaveEnemies.Add(waveEnemy);
            }
        }
#endif
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

        public void ClearEnemyInfoCache()
        {
            _enemyInfoCache.Clear();
        }
    }
}