using Cysharp.Threading.Tasks;
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
        private MainGamePlaySystem _mainGamePlaySystem => Locator<MainGamePlaySystem>.Instance;
        
        private bool _isHaveEnemyToAttack;
        private bool _isEndWave;

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

        private void CheckUserWin()
        {
            if (!_isHaveEnemyToAttack && _isEndWave)
            {
                GetStarWin();
                IsUserWin?.Invoke(true);
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
            if (hpEnd >= twoThirdsHp)
            {
                StarWin = 2;
                return;
            }
            
            else StarWin = 1;
        }

        private void UserLose()
        {
            IsUserWin?.Invoke(false);
        }

        private void OnDisable()
        {
            _mainGamePlaySystem.SpawnEnemiesHandler.HaveEnemyToAttack -= HaveEnemyToAttack;
            _mainGamePlaySystem.SpawnEnemiesHandler.EndWave -= EndWave;
            _mainGamePlaySystem.UserRecieveDamageHandler.IsDead -= UserLose;
        }
    }
}