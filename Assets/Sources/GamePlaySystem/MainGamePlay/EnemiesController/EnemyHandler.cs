using Cysharp.Threading.Tasks;
using Game.Character.Leader;
using Game.PosSpawnEnemies;
using Sources.DataBaseSystem;
using Sources.Extension;
using Sources.GamePlaySystem.CoinController;
using Sources.Utils;
using Sources.Utils.Singleton;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace Sources.GamePlaySystem.MainGamePlay.Enemies
{
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
        private const int _timeDelayAfterAttackUser = 1000;
        private const int _criticalRate = 100; //50%
        private const int _factorCritical = 2;

        private DataBase _dataBase => Locator<DataBase>.Instance;
        private EnemiesConfig _enemiesConfig => _dataBase.GetConfig<EnemiesConfig>();
        private MainGamePlaySystem _mainGamePlaySystem => Locator<MainGamePlaySystem>.Instance;
        
        private IDisposable _disposableShieldState;

        public int HpMax { get; private set; }
        public ReactiveProperty<int> HpCurrent { get; } = new ReactiveProperty<int>();
        public ReactiveProperty<int> Damage { get; } = new ReactiveProperty<int>();
        public ReactiveProperty<Vector2> Direction { get; } = new ReactiveProperty<Vector2>(Vector2.zero);
        public ReactiveProperty<AnimationState> AniamtionState { get;} = new ReactiveProperty<AnimationState>(AnimationState.Idle);
        public ReactiveProperty<bool> IsAttacking { get;} = new ReactiveProperty<bool>(false);
        public Action<int> DamageFeed;
        public int CoinsReward { get; private set; }

        public void OnSetUp(string enemyId)
        {
            GetEnemyInfo(enemyId);
            OnWalk();
            SubscribeShieldState();
            SubscribeUserDeath();
        }

        private void GetEnemyInfo(string enemyId)
        {
            var enemy = _enemiesConfig.GetEnemyInfo(enemyId);
            var enemyWaveInfo = enemy.GetWaveEnemy(_mainGamePlaySystem.SpawnEnemiesHandler.WaveIdCurrent);

            HpCurrent.Value = HpMax = enemyWaveInfo.Hp;
            Damage.Value = enemyWaveInfo.Damage;

            CoinsReward = GetRandom.GetCoinRandom(enemyWaveInfo.coinReward, enemyWaveInfo.PercentChance);
        }

        private void OnWalk()
        {
            AniamtionState.Value = AnimationState.Walk;
            Direction.Value = Vector2.left;
        }
        
        private void SubscribeShieldState()
        {
            _disposableShieldState = _mainGamePlaySystem.UserRecieveDamageHandler.ShieldCurrentState.Subscribe(value =>
            {
                if (value == ShieldState.Empty)
                {
                    OnWalk();
                }
            });
        }

        private void SubscribeUserDeath()
        {
            _mainGamePlaySystem.UserRecieveDamageHandler.IsDead += OnIdle;
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

            HpCurrent.Value -= damageRecieve;
            CheckDeath();
        }

        private void CheckDeath()
        {
            if (HpCurrent.Value <= 0)
            {
                OnDeath();
            }
        }

        private void OnDeath()
        {
            Direction.Value = Vector2.zero;
            AniamtionState.Value = AnimationState.Death;
        }

        public void OnAttack()
        {
            IsAttacking.Value = true;

            OnIdle();

            AniamtionState.Value = AnimationState.Attack;
        }

        private void OnIdle()
        {
            AniamtionState.Value = AnimationState.Idle;
            Direction.Value = Vector2.zero;
        }

        public async void DamageUser()
        {
            AniamtionState.Value = AnimationState.Idle;
            _mainGamePlaySystem.UserRecieveDamageHandler.SubstractHp(Damage.Value);

            await UniTask.Delay(_timeDelayAfterAttackUser);

            IsAttacking.Value = false;
        }

        public void Disposable()
        {
            _disposableShieldState.Dispose();
            _mainGamePlaySystem.UserRecieveDamageHandler.IsDead -= OnIdle;
        }
    }
}