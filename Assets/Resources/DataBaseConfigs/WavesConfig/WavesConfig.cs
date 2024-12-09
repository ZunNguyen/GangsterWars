using Resources.CSV;
using Sirenix.OdinInspector;
using Sources.Utils.String;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Sources.DataBaseSystem
{
    [Serializable]
    public class Wave
    {
        public string Id;
        public int TotalHp;
        public List<Turn> Turns;

        private string GetDescription()
        {
            return Id;
        }
    }

    [Serializable]
    public class Turn
    {
        public string Id;
        public List<Phase> Phases;
    }

    [Serializable]
    public class Phase
    {
        public string PhaseId;
        public Enemy Enemy;
        public int SpawnAfterMiliSeccond;
    }

    [Serializable]
    public class Enemy
    {
        public string EnemyId;
        public List<int> IndexPos;
    }

    [Serializable]
    public class WaveInfo
    {
        public string WaveId;
        public int CoinRewards;
        [PreviewField(75, ObjectFieldAlignment.Center)]
        public Sprite Sprite;

        private string GetDescription()
        {
            return WaveId;
        }
    }

    public class WavesConfig : DataBaseConfig , IReadCSVData
    {
        [TabGroup("Wave Spawn")]
        [SerializeField, ListDrawerSettings(ListElementLabelName = "GetDescription")]
        private List<Wave> _waveSpawn;
        public Dictionary<string, Wave> WaveSpawnCache = new();

        [TabGroup("Wave Info")]
        [SerializeField, ListDrawerSettings(ListElementLabelName = "GetDescription")]
        private List<WaveInfo> _waveMoreInfos = new();
        private Dictionary<string, WaveInfo> _waveMoreInfoCache = new();

        [SerializeField, ReadOnly, TabGroup("Wave Spawn")]
        private TextAsset CSVFileWaveSpawn;

        [SerializeField, ReadOnly, TabGroup("Wave Info")]
        private TextAsset CSVFileReward;

        public Wave GetSpawnWaveInfo(string id)
        {
            if (!WaveSpawnCache.ContainsKey(id))
            {
                var wave = _waveSpawn.Find(waveTarget => waveTarget.Id == id);
                WaveSpawnCache.Add(id, wave);
            }

            return WaveSpawnCache[id];
        }

        public int GetIndexWaveInfo(string id)
        {
            var waveInfo = GetSpawnWaveInfo(id);
            return _waveSpawn.IndexOf(waveInfo);
        }

        public Wave GetWaveInfo(int index)
        {
            return _waveSpawn[index];
        }

        public WaveInfo GetBGWaveInfo(string waveId)
        {
            if (!_waveMoreInfoCache.ContainsKey(waveId))
            {
                var waveInfo = _waveMoreInfos.Find(x => x.WaveId == waveId);
                _waveMoreInfoCache.Add(waveId, waveInfo);
            }

            return _waveMoreInfoCache[waveId];
        }

#if UNITY_EDITOR
        private const int _startIndexRowData = 1;

        private int _totalHpInWave;

        private string[] _datas;
        private int _rowCount;
        private int _columnCount;

        private EnemiesConfig _enemiesConfig;

        [Button, TabGroup("Wave Spawn")]
        public void ReadFile()
        {
            _waveSpawn.Clear();

            _datas = CSVFileWaveSpawn.text.Split(new string[] { ",", "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries);
            string[] lines = CSVFileWaveSpawn.text.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);

            _rowCount = lines.Length;
            _columnCount = _datas.Length / _rowCount;

            GetEnemiesConfig();
            ProcessWave();
        }

        private void GetEnemiesConfig()
        {
            string pathEnemiesConfig = "Assets/Resources/DataBaseConfigs/EnemiesConfig/EnemiesConfig.asset";
            _enemiesConfig = AssetDatabase.LoadAssetAtPath<EnemiesConfig>(pathEnemiesConfig);

            _totalHpInWave = 0;
            _enemiesConfig.ClearEnemyInfoCache();
        }

        private void ProcessWave()
        {
            int countSameWave = 0;
            var startIndexWave = _columnCount;
            var startWaveId = _datas[startIndexWave];
            for (int row = _startIndexRowData; row < _rowCount; row++)
            {
                var indexWave = row * _columnCount;
                if (_datas[indexWave] != startWaveId)
                {
                    var starIndexWave = row - countSameWave;
                    var endIndexWave = row;

                    var newWave = new Wave
                    {
                        Id = startWaveId,
                        Turns = new()
                    };
                    _waveSpawn.Add(newWave);
                    ProcessTurn(newWave, starIndexWave, endIndexWave);

                    countSameWave = 0;
                    startWaveId = _datas[indexWave];

                    newWave.TotalHp = _totalHpInWave;
                    _totalHpInWave = 0;
                }
                countSameWave++;
            }
        }

        private void ProcessTurn(Wave wave, int startIndex, int endIndex)
        {
            int countSameTurn = 0;
            var startIndexTurn = _columnCount * startIndex + 1;
            var startTurnId = _datas[startIndexTurn];
            for (int row = startIndex; row <= endIndex; row++)
            {
                var indexTurn = row * _columnCount + 1;
                if (_datas[indexTurn] != startTurnId)
                {
                    var starIndexTurn = row - countSameTurn;
                    var endIndexTurn = row - 1;

                    var newTurn = new Turn
                    {
                        Id = startTurnId,
                        Phases = new()
                    };
                    wave.Turns.Add(newTurn);
                    ProcessPhase(newTurn, starIndexTurn, endIndexTurn);

                    countSameTurn = 0;
                    startTurnId = _datas[indexTurn];
                }
                countSameTurn++;
            }
        }

        private void ProcessPhase(Turn wave, int startIndex, int endIndex)
        {
            int startIndexColPhase = 2;
            int startIndexColTimeSpawn = 5;
                
            for (int row = startIndex; row <= endIndex; row++)
            {
                var indexPhase = row * _columnCount + startIndexColPhase;
                var indexTimeSpawn = row * _columnCount + startIndexColTimeSpawn;
                var phase = new Phase
                {
                    PhaseId = _datas[indexPhase],
                    SpawnAfterMiliSeccond = int.Parse(_datas[indexTimeSpawn])
                };

                wave.Phases.Add(phase);
                ProcessEnemyData(phase, row);
            }
        }

        private void ProcessEnemyData(Phase phase, int indexRow)
        {
            int startIndexColEnemy = 3;
            int startIndexColPosSpawn = 4;

            var indexEnemy = indexRow * _columnCount + startIndexColEnemy;
            var indexPosSpawn = indexRow * _columnCount + startIndexColPosSpawn;

            phase.Enemy = new Enemy
            {
                EnemyId = _datas[indexEnemy],
                IndexPos = new()
            };

            var posSpawns = StringUtils.SeparateString_1(_datas[indexPosSpawn]);
            foreach (var posSpawn in posSpawns)
            {
                var convertPosSpawn = int.Parse(posSpawn);
                phase.Enemy.IndexPos.Add(convertPosSpawn);
            }

            // Get TotalHp
            var enemyInfo = _enemiesConfig.GetEnemyInfo(phase.Enemy.EnemyId);
            var hpEnemy = enemyInfo.WaveEnemies[_waveSpawn.Count - 1].Hp;
            var countEnemy = phase.Enemy.IndexPos.Count;
            _totalHpInWave = _totalHpInWave + (hpEnemy * countEnemy);
        }
#endif

#if UNITY_EDITOR

        [Button, TabGroup("Wave Info")]
        public void ReadFileWaveReward()
        {
            _datas = CSVFileReward.text.Split(new string[] { ",", "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries);
            string[] lines = CSVFileReward.text.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);

            _rowCount = lines.Length;
            _columnCount = _datas.Length / _rowCount;

            for (int i = 0; i < _waveMoreInfos.Count; i++)
            {
                var reward = _datas[_columnCount * (i + 1) + 1];
                _waveMoreInfos[i].CoinRewards = int.Parse(reward);
            }
        }

#endif

//#if UNITY_EDITOR
//        [Button]
//        public void Creat()
//        {
//            Clear();

//            string waveNum = "";
//            for (int i = 1; i <= 40; i++)
//            {
//                var wave = new WaveInfo();
//                if (i < 10)
//                {
//                    waveNum = "0" + i.ToString();
//                }
//                else
//                {
//                    waveNum = i.ToString();
//                }

//                wave.WaveId = $"wave-{waveNum}";
//                _waveMoreInfos.Add(wave);
//            }
//        }

//        [Button]
//        public void Clear()
//        {
//            _waveMoreInfos.Clear();
//        }

//        [Button]
//        public void AddSprite(Sprite sprite, int indexBegin, int indexFinish)
//        {
//            for (int i = indexBegin - 1; i < indexFinish; i++)
//            {
//                _waveMoreInfos[i].Sprite = sprite;
//            }
//        }
//#endif
    }
}