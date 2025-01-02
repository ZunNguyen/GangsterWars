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
    public enum ActionType
    {
        None,
        Hit,
        Shoot
    }

    public enum DamageUserType
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

        private int _qualityWeaponShoot;
        private bool _isHaveColliderInFace;
        private bool _isEndGame;
        private EnemyInfo _enemyInfo;
        private IDisposable _disposableShieldState;
        private ReloadTimeHandler _reloadTimeHandler;

        public int CoinsReward { get; private set; }
        public int HpMax { get; private set; }
        
        public ReactiveProperty<int> HpCurrent { get; private set; } = new ();
        public ReactiveProperty<int> Damage { get; private set; } = new ();
        public ReactiveProperty<bool> IsAttacking { get; private set; } = new (false);
        public ReactiveProperty<Vector2> Direction { get; private set; } = new (Vector2.zero);
        public ReactiveProperty<AnimationState> AniamtionState { get; private set; } = new (AnimationState.Idle);
        public ReactiveProperty<ActionType> ActionTypeCurrent { get; private set; } = new (Enemies.ActionType.None);

        public Action<int> DamageFeed;
        
        public EnemyHandler()
        {
            SubscribeShieldState();
            SubscribeUserDeath();
            _reloadTimeHandler = new(ActionTypeCurrent);
        }

        private void SubscribeShieldState()
        {
            _disposableShieldState = _mainGamePlaySystem.UserRecieveDamageHandler.ShieldCurrentState.Subscribe(value =>
            {
                if (value == ShieldState.Empty && ActionTypeCurrent.Value == ActionType.Hit)
                {
                    _isHaveColliderInFace = false;
                    IsAttacking.Value = false;
                    OnAction();
                }
            });
        }

        private void SubscribeUserDeath()
        {
            _gameResultSystem.IsUserWin += EndGame;
        }

        public void OnSetUp(string enemyId)
        {
            _isHaveColliderInFace = false;
            _isEndGame = false;
            IsAttacking.Value = false;
            _reloadTimeHandler.SetEnemyId(enemyId);

            GetEnemyInfo(enemyId);
            OnAction();
        }

        private void GetEnemyInfo(string enemyId)
        {
            _enemyInfo = _enemiesConfig.GetEnemyInfo(enemyId);
            var enemyWaveInfo = _enemyInfo.GetWaveEnemy(_mainGamePlaySystem.SpawnEnemiesHandler.WaveIdCurrent);

            HpCurrent.Value = HpMax = enemyWaveInfo.Hp;
            Damage.Value = enemyWaveInfo.Damage;

            CoinsReward = GetRandom.GetCoinRandom(enemyWaveInfo.coinReward, enemyWaveInfo.PercentChance);

            if (_enemyInfo.IsCanShoot) _qualityWeaponShoot = _enemyInfo.QualityWeaponShoot;
            else _qualityWeaponShoot = _enemyInfo.QualityHit;
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

        public void HaveColliderInFace(string collisionKey)
        {
            _isHaveColliderInFace = true;

            if (collisionKey == CollisionTagKey.STOP_POINT_SHOOT && _enemyInfo.IsCanShoot) 
                ActionTypeCurrent.Value = Enemies.ActionType.Shoot;
            
            if (collisionKey == CollisionTagKey.SHIELD_USER && _enemyInfo.IsCanHit) 
                ActionTypeCurrent.Value = Enemies.ActionType.Hit;

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

        public void DamageUser(DamageUserType type)
        {
            _mainGamePlaySystem.UserRecieveDamageHandler.SubstractHp(Damage.Value, type);
        }

        public async void CompleteActionAttack()
        {
            AniamtionState.Value = AnimationState.Idle;

            SubstractWeaponShoot();

            await _reloadTimeHandler.ReloadingTime();
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

        private void SubstractWeaponShoot()
        {
            _qualityWeaponShoot--;
            if (_qualityWeaponShoot <= 0) _isHaveColliderInFace = false;
        }

        private void OnDestroy()
        {
            _disposableShieldState?.Dispose();
            _gameResultSystem.IsUserWin -= EndGame;
        }
    }
}