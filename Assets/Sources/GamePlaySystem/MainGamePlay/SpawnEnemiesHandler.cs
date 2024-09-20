using Cysharp.Threading.Tasks;
using Sources.DataBaseSystem;
using Sources.Utils.Singleton;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace Sources.GamePlaySystem.MainGamePlay
{
    public class SpawnEnemiesHandler
    {
        private GameData.GameData _gameData => Locator<GameData.GameData>.Instance;

        private DataBase _dataBase => Locator<DataBase>.Instance;
        private EnemySpawnConfig _enemySpawnConfig => Locator<EnemySpawnConfig>.Instance;

        private string _waveId;
        private Wave _waveInfo;
        private int _turnIndexCurrent = 0;
        private int _phaseIndexCurrent = 0;
        private bool _endWave = false;

        public ReactiveProperty<Enemy> EnemyModel { get; private set; } = new();
        public ReactiveProperty<int> CountEnemy { get; private set; } = new();

        public void SetWaveId(string id)
        {
            _waveId = id;
            GetWaveInfo();
        }

        public void GetWaveInfo()
        {
            _waveInfo = _enemySpawnConfig.GetWaveInfo(_waveId);
            GetMaxEnemy();
        }

        public async void SpawnEnemies()
        {
            while (!_endWave)
            {
                var phaseCurrent = _waveInfo.Turns[_turnIndexCurrent].Phases[_phaseIndexCurrent];
                await UniTask.Delay(phaseCurrent.SpawnAfterMiliSeccond);
                EnemyModel.Value = phaseCurrent.Enemy;

                UpdateIndex();
                CheckEndWave();
            }
        }

        private void CheckEndWave()
        {
            var turnIndexMax = _waveInfo.Turns.Count - 1;
            if (_turnIndexCurrent > turnIndexMax) _endWave = true;
        }

        private void UpdateIndex()
        {
            var phaseIndexMax = _waveInfo.Turns[_turnIndexCurrent].Phases.Count - 1;
            _phaseIndexCurrent++;
            if (_phaseIndexCurrent > phaseIndexMax)
            {
                _phaseIndexCurrent = 0;
                _turnIndexCurrent++;
            }
        }

        private void GetMaxEnemy()
        {
            for (int turn = 0; turn < _waveInfo.Turns.Count; turn++)
            {
                for (int phase = 0; phase < _waveInfo.Turns[turn].Phases.Count; phase++)
                {
                    var enemyCount = _waveInfo.Turns[turn].Phases[phase].Enemy.IndexPos.Count;
                    CountEnemy.Value += enemyCount;
                }
            }
        }
    }
}