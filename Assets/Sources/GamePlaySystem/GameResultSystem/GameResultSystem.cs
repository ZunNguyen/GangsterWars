using Cysharp.Threading.Tasks;
using Sources.DataBaseSystem;
using Sources.Extension;
using Sources.GameData;
using Sources.GamePlaySystem.MainGamePlay;
using Sources.SystemService;
using Sources.Utils.Singleton;
using System;
using Unity.VisualScripting;

namespace Sources.GamePlaySystem.GameResult
{
    public class InitGameResultSystemService : InitSystemService<GameResultSystem> { }

    public class GameResultSystem : BaseSystem
    {
        private GameData.GameData _gameData => Locator<GameData.GameData>.Instance;
        private JourneyProfile _jourNeyProfile => _gameData.GetProfileData<JourneyProfile>();

        private DataBase _dataBase => Locator<DataBase>.Instance;
        private SpawnWaveConfig _spawnWaveConfig => _dataBase.GetConfig<SpawnWaveConfig>();

        private MainGamePlaySystem _mainGamePlaySystem => Locator<MainGamePlaySystem>.Instance;
        
        private bool _isHaveEnemyToAttack;
        private bool _isEndWave;

        public string WaveIdCurrent { get; private set; }
        public string WaveIdNext { get; private set; }
        public int StarWin { get; private set; }
        public Action<bool> IsUserWin;

        public override async UniTask Init()
        {
            
        }

        public void OnSetUp()
        {
            OnDisable();

            _isEndWave = false;
            _isHaveEnemyToAttack = true;

            _mainGamePlaySystem.SpawnEnemiesHandler.HaveEnemyToAttack += HaveEnemyToAttack;
            _mainGamePlaySystem.SpawnEnemiesHandler.EndWave += EndWave;
            _mainGamePlaySystem.UserRecieveDamageHandler.IsDead += UserLose;
        }

        private void HaveEnemyToAttack(bool value)
        {
            _isHaveEnemyToAttack = value;

            if (!_isHaveEnemyToAttack) CheckUserWin();
        }

        private void EndWave()
        {
            _isEndWave = true;
            CheckUserWin();
        }
        private void UserLose()
        {
            IsUserWin?.Invoke(false);
        }

        private void CheckUserWin()
        {
            if (!_isHaveEnemyToAttack && _isEndWave)
            {
                IsUserWin?.Invoke(true);
                GetStarWin();
                GetWaveIdCurrent();
                GetWaveIdNext();
            }
        }

        private void GetStarWin()
        {
            var hpBegin = _mainGamePlaySystem.UserRecieveDamageHandler.MaxHpBegin;
            var hpEnd = _mainGamePlaySystem.UserRecieveDamageHandler.GetTotalHpWhenEnd();

            var twoThirdsHp = (float)hpBegin * 2 / 3;
            if (hpEnd >= twoThirdsHp)
            {
                StarWin = 3;
                return;
            }

            var oneThirdsHp = (float)hpBegin * 1 / 3;
            if (hpEnd >= oneThirdsHp)
            {
                StarWin = 2;
                return;
            }
            
            else StarWin = 1;
        }

        private void GetWaveIdCurrent()
        {
            WaveIdCurrent = _mainGamePlaySystem.WaveIdCurrent;
        }

        private void GetWaveIdNext()
        {
            if (WaveIdCurrent == WaveKey.WAVE_ID_MAX)
            {
                WaveIdNext = WaveKey.WAVE_ID_MAX;
                return;
            }

            var indexWaveCurrent = _spawnWaveConfig.GetIndexWaveInfo(WaveIdCurrent);
            var waveNexInfo = _spawnWaveConfig.GetWaveInfo(++indexWaveCurrent);
            WaveIdNext = waveNexInfo.Id;
        }

        private void OnDisable()
        {
            _mainGamePlaySystem.SpawnEnemiesHandler.HaveEnemyToAttack -= HaveEnemyToAttack;
            _mainGamePlaySystem.SpawnEnemiesHandler.EndWave -= EndWave;
            _mainGamePlaySystem.UserRecieveDamageHandler.IsDead -= UserLose;
        }
    }
}