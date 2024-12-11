using Cysharp.Threading.Tasks;
using Sources.Audio;
using Sources.DataBaseSystem;
using Sources.Extension;
using Sources.GamePlaySystem.GameResult;
using Sources.Utils;
using Sources.Utils.Singleton;
using System;
using UniRx;
using UnityEngine;

namespace Sources.GamePlaySystem.MainGamePlay.Enemies
{
    public enum TypeDamageUser
    {
        LongRangeDamage,
        ShortRangeDamage
    }

    public enum AnimationState
    {
        None,
        Idle,
        Walk,
        Attack,
        Death
    }

    public static class AnimationStateEx
    {
        public static string ConvertToString(this AnimationState state)
        {
            return state switch
            {
                AnimationState.None => "None",
                AnimationState.Idle => "Idle",
                AnimationState.Walk => "Walk",
                AnimationState.Attack => "Attack",
                AnimationState.Death => "Death",

                _ => throw new ArgumentOutOfRangeException(nameof(state), state, null)
            };
        }
    }

    public class EnemyHandler
    {
        private const int _criticalRate = 50; //50%
        private const int _factorCritical = 2;

        private DataBase _dataBase => Locator<DataBase>.Instance;
        private EnemiesConfig _enemiesConfig => _dataBase.GetConfig<EnemiesConfig>();
        private MainGamePlaySystem _mainGamePlaySystem => Locator<MainGamePlaySystem>.Instance;
        private GameResultSystem _gameResultSystem => Locator<GameResultSystem>.Instance;
        private AudioManager _audioManager => Locator<AudioManager>.Instance;

        private bool _isHaveColliderInFace;
        private bool _isEndGame;
        private int _timeReload;
        public int CoinsReward { get; private set; }
        public int HpMax { get; private set; }
        private IDisposable _disposableShieldState;

        public ReactiveProperty<int> HpCurrent { get; private set; } = new ();
        public ReactiveProperty<int> Damage { get; private set; } = new ();
        public ReactiveProperty<bool> IsAttacking { get; private set; } = new (false);
        public ReactiveProperty<Vector2> Direction { get; private set; } = new (Vector2.zero);
        public ReactiveProperty<AnimationState> AniamtionState { get; private set; } = new (AnimationState.Idle);

        public Action<int> DamageFeed;
        
        public void SetUpFirst()
        {
            SubscribeShieldState();
            SubscribeUserDeath();
        }

        private void SubscribeShieldState()
        {
            _disposableShieldState = _mainGamePlaySystem.UserRecieveDamageHandler.ShieldCurrentState.Subscribe(value =>
            {
                if (value == ShieldState.Empty) OnAction();
            });
        }

        private void SubscribeUserDeath()
        {
            _gameResultSystem.IsUserWin += EndGame;
        }

        public void OnSetUp(string enemyId)
        {
            GetEnemyInfo(enemyId);
            _isHaveColliderInFace = false;
            _isEndGame = false;
            OnAction();
        }

        private void GetEnemyInfo(string enemyId)
        {
            var enemy = _enemiesConfig.GetEnemyInfo(enemyId);
            var enemyWaveInfo = enemy.GetWaveEnemy(_mainGamePlaySystem.SpawnEnemiesHandler.WaveIdCurrent);

            HpCurrent.Value = HpMax = enemyWaveInfo.Hp;
            Damage.Value = enemyWaveInfo.Damage;

            CoinsReward = GetRandom.GetCoinRandom(enemyWaveInfo.coinReward, enemyWaveInfo.PercentChance);

            _timeReload = enemy.TimeToReload;
        }

        private void OnAction()
        {
            if (_isEndGame) return;

            if (_isHaveColliderInFace)
            {
                SetIdle();
                Attack();
            }
            else SetWalk();
        }

        public void HaveColliderInFace()
        {
            _isHaveColliderInFace = true;
            OnAction();
        }

        private void EndGame(bool isUserWin)
        {
            _isEndGame = true;
            if (!isUserWin) SetIdle();
        }

        private void Attack()
        {
            IsAttacking.Value = true;
            AniamtionState.Value = AnimationState.Attack;
        }

        private void SetIdle()
        {
            AniamtionState.Value = AnimationState.Idle;
            Direction.Value = Vector2.zero;
        }

        private void SetWalk()
        {
            AniamtionState.Value = AnimationState.Walk;
            Direction.Value = Vector2.left;
        }

        public async void DamageUser(TypeDamageUser type)
        {
            AniamtionState.Value = AnimationState.Idle;
            _mainGamePlaySystem.UserRecieveDamageHandler.SubstractHp(Damage.Value, type);

            await UniTask.Delay(_timeReload);
            IsAttacking.Value = false;

            OnAction();
        }

        public void SubstractHp(int damage, string collision)
        {
            var damageRecieve = damage;
            if (collision == CollisionTagKey.ENEMY_HEAD)
            {
                var citiricalRateCurrent = GetRandom.GetRandomCriticalRate();
                if (citiricalRateCurrent <= _criticalRate)
                {
                    damageRecieve = damage * _factorCritical;
                    DamageFeed?.Invoke(damageRecieve);
                }
            }

            var substractHp = Math.Min(HpCurrent.Value, damageRecieve);
            _mainGamePlaySystem.EnemiesController.SubstractHpTotal(substractHp);

            HpCurrent.Value -= damageRecieve;
            HpCurrent.Value = Math.Max(0, HpCurrent.Value);

            CheckDeath();
        }

        private void CheckDeath()
        {
            if (HpCurrent.Value <= 0)
            {
                _mainGamePlaySystem.EnemiesController.MoveToAvailableList(this);
                Death();
            }
        }

        private void Death()
        {
            _audioManager.Play(AudioKey.SFX_ENEMY_DEATH);
            Direction.Value = Vector2.zero;
            AniamtionState.Value = AnimationState.Death;
        }

        private void OnDestroy()
        {
            _disposableShieldState?.Dispose();
            _gameResultSystem.IsUserWin -= EndGame;
        }
    }
}