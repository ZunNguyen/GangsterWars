using Cysharp.Threading.Tasks;
using Game.Character.Enemy.Abstract;
using Sources.DataBaseSystem;
using Sources.Utils.Singleton;
using System;
using System.Collections.Generic;
using UniRx;

namespace Sources.GamePlaySystem.MainGamePlay
{
    public class SpawnEnemiesHandler
    {
        private DataBase _dataBase => Locator<DataBase>.Instance;
        private WavesConfig _enemySpawnConfig => _dataBase.GetConfig<WavesConfig>();

        private MainGamePlaySystem _mainGamePlaySystem => Locator<MainGamePlaySystem>.Instance;

        private Wave _waveInfo;
        private int _turnIndexCurrent = 0;
        private int _phaseIndexCurrent = 0;
        private bool _endWave = false;
        private string _waveIdCurrent;
        public string WaveIdCurrent => _waveIdCurrent;

        public ReactiveProperty<Enemy> EnemyModel { get; private set; } = new();
        public List<EnemyControllerAbstract> Enemies { get; private set; } = new();
        public Action<bool> HaveEnemyToAttack;
        public Action EndWave;

        public void OnSetUp(string id)
        {
            _waveIdCurrent = id;
            GetWaveInfo();
            _mainGamePlaySystem.UserRecieveDamageHandler.IsDead += SetEndWave;
        }

        private void SetEndWave()
        {
            _endWave = true;
        }

        private void GetWaveInfo()
        {
            _waveInfo = _enemySpawnConfig.GetSpawnWaveInfo(_waveIdCurrent);
            GetMaxEnemy();
        }

        public async void SpawnEnemies()
        {
            await UniTask.Delay(2000);

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
            if (_turnIndexCurrent > turnIndexMax)
            {
                EndWave?.Invoke();
                _endWave = true;
            }
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
                }
            }
        }

        public void AddEnemyToList(EnemyControllerAbstract enemy)
        {
            HaveEnemyToAttack?.Invoke(true);
            Enemies.Add(enemy);
        }

        public void RemoveEnemyToList(EnemyControllerAbstract enemy)
        {
            Enemies.Remove(enemy);
            if (Enemies.Count > 0) HaveEnemyToAttack?.Invoke(true);
            if (Enemies.Count == 0) HaveEnemyToAttack?.Invoke(false);
        }

        private void OnDestroy()
        {
            _mainGamePlaySystem.UserRecieveDamageHandler.IsDead -= SetEndWave;
        }
    }
}