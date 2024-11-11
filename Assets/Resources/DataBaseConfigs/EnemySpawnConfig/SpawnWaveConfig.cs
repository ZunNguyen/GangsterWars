using BestHTTP.SecureProtocol.Org.BouncyCastle.Bcpg.Sig;
using Resources.CSV;
using Sirenix.OdinInspector;
using Sources.DataBaseSystem;
using Sources.Utils.String;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Sources.DataBaseSystem
{
    [Serializable]
    public class Wave
    {
        public string Id;
        public List<Turn> Turns;
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

    public class SpawnWaveConfig : DataBaseConfig , IReadCSVData
    {
        [SerializeField] private List<Wave> _waves;
        public Dictionary<string, Wave> WavesCache = new();

        public TextAsset CSVFile;

        public Wave GetWaveInfo(string id)
        {
            if (!WavesCache.ContainsKey(id))
            {
                var wave = _waves.Find(waveTarget => waveTarget.Id == id);
                WavesCache.Add(id, wave);
            }

            return WavesCache[id];
        }

        public int GetIndexWaveInfo(string id)
        {
            var waveInfo = GetWaveInfo(id);
            return _waves.IndexOf(waveInfo);
        }

#if UNITY_EDITOR
        private const int _startIndexRowData = 1;

        private string[] _datas;
        private int _rowCount;
        private int _columnCount;

        [Button]
        public void ReadFile()
        {
            _waves.Clear();
            _datas = CSVFile.text.Split(new string[] { ",", "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries);
            string[] lines = CSVFile.text.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);

            _rowCount = lines.Length;
            _columnCount = _datas.Length / _rowCount;

            ProcessWave();
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
                    _waves.Add(newWave);
                    ProcessTurn(newWave, starIndexWave, endIndexWave);
                    
                    countSameWave = 0;
                    startWaveId = _datas[indexWave];
                    break;
                }
                countSameWave++;
            }
        }

        private void ProcessTurn(Wave wave, int startIndex, int endIndex)
        {
            int countSameTurn = 0;
            var startIndexTurn = _columnCount + 1;
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
                    break;
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

            var posSpawns = StringUtils.SeparateString(_datas[indexPosSpawn]);
            foreach (var posSpawn in posSpawns)
            {
                var convertPosSpawn = int.Parse(posSpawn);
                phase.Enemy.IndexPos.Add(convertPosSpawn);
            }
        }
    }
#endif
}