using Sirenix.OdinInspector;
using Sources.Utils.String;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Sources.DataBaseSystem
{
    [Serializable]
    public class EnemyInfo
    {
        public string Id;
        [PreviewField(100, ObjectFieldAlignment.Center)]
        public GameObject EnemyPrefab;

        public bool IsCanHit;
        [ShowIf(nameof(IsCanHit))] public int QualityHit;
        [ShowIf(nameof(IsCanHit))] public int TimeToReloadHit;

        public bool IsCanShoot;
        [ShowIf(nameof(IsCanShoot))] public int QualityWeaponShoot;
        [ShowIf(nameof(IsCanShoot))] public int TimeToReloadShoot;

        [ListDrawerSettings(ListElementLabelName = "GetDescription")]
        public List<WaveEnemy> WaveEnemies = new();
        public Dictionary<string, WaveEnemy> WaveEnemyCache { get; private set; } = new();

        [SerializeField] private TextAsset _csvFile;

        public WaveEnemy GetWaveEnemy(string id)
        {
            if (!WaveEnemyCache.ContainsKey(id))
            {
                var waveEnemy = WaveEnemies.Find(x => x.Id == id);
                WaveEnemyCache.Add(id, waveEnemy);
            }
            return WaveEnemyCache[id];
        }

        private string GetDiscription()
        {
            return Id;
        }

#if UNITY_EDITOR
        private const int _startRowIndex = 1;
        private const int _startIndexColWaveId = 0;
        private const int _startIndexColDamage = 1;
        private const int _startIndexColHp = 2;
        private const int _startIndexColCoinReward = 3;
        private const int _startIndexColPerChance = 4;

        [Button]
        public void ReadFile()
        {
            WaveEnemies.Clear();
            string[] datas = _csvFile.text.Split(new string[] { ",", "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries);
            string[] lines = _csvFile.text.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);

            var rowCount = lines.Length;
            var columnCount = datas.Length / rowCount;

            for (int row = _startRowIndex; row < rowCount; row++)
            {
                var indexId = row * columnCount + _startIndexColWaveId;
                var indexDamage = row * columnCount + _startIndexColDamage;
                var indexHp = row * columnCount + _startIndexColHp;
                var indexCoinReward = row * columnCount + _startIndexColCoinReward;
                var indexPerChance = row * columnCount + _startIndexColPerChance;

                var coinReward = StringUtils.SeparateString_2(datas[indexCoinReward]);
                var coinRewardMin = int.Parse(coinReward[0]);
                var coinRewardMax = int.Parse(coinReward[1]);

                var waveEnemy = new WaveEnemy
                {
                    Id = datas[indexId],
                    Damage = int.Parse(datas[indexDamage]),
                    Hp = int.Parse(datas[indexHp]),
                    coinReward = new Vector2Int(coinRewardMin, coinRewardMax),
                    PercentChance = int.Parse(datas[indexPerChance])
                };

                WaveEnemies.Add(waveEnemy);
            }
        }

        private const string _filePath = "Assets/Resources/CSV/Enemies";
        [Button]
        public void SaveFile()
        {
            List<string> csvLines = new();
            csvLines.Add("WaveId, Damage, Hp, CoinRewardMin-CoinRewardMax, PercentChance");

            foreach (WaveEnemy waveEnemy in WaveEnemies)
            {
                string line = $"{waveEnemy.Id}," +
                              $"{waveEnemy.Damage}," +
                              $"{waveEnemy.Hp}," +
                              $"{waveEnemy.coinReward.x}&{waveEnemy.coinReward.y}," +
                              $"{waveEnemy.PercentChance}";

                csvLines.Add(line);
            }

            string filePath = $"{_filePath}/{_csvFile.name}.csv";

            File.WriteAllLines(filePath, csvLines);
            AssetDatabase.Refresh();
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

        private string GetDescription()
        {
            return Id;
        }
    }

    public class EnemiesConfig : DataBaseConfig
    {
        [SerializeField, ListDrawerSettings(ListElementLabelName = "GetDiscription")]
        private List<EnemyInfo> _enemies;
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