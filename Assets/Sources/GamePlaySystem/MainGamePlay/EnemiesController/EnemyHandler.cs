using Cysharp.Threading.Tasks;
using Game.Character.Leader;
using Sources.DataBaseSystem;
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
        private MainGamePlaySystem _mainGamePlaySystem => Locator<MainGamePlaySystem>.Instance;

        public int HpMax { get; private set; }
        public string EnemyId { get; private set; }
        public ReactiveProperty<int> HpCurrent { get; } = new ReactiveProperty<int>();
        public ReactiveProperty<int> Damage { get; } = new ReactiveProperty<int>();
        public ReactiveProperty<Vector2> Direction { get; } = new ReactiveProperty<Vector2>(Vector2.zero);
        public ReactiveProperty<AnimationState> AniamtionState { get;} = new ReactiveProperty<AnimationState>(AnimationState.Idle);
        public ReactiveProperty<bool> IsAttacking { get;} = new ReactiveProperty<bool>(false);

        public void OnSetUp(Enemy enemyInfo)
        {
            EnemyId = enemyInfo.EnemyId;
            HpCurrent.Value = HpMax = enemyInfo.Hp;
            Damage.Value = enemyInfo.Damage;

            OnStart();
        }

        private void OnStart()
        {
            AniamtionState.Value = AnimationState.Walk;
            Direction.Value = Vector2.left;
        }

        public void SubstractHp(int hp)
        {
            HpCurrent.Value -= hp;
            CheckDeath();
        }

        private void CheckDeath()
        {
            if (HpCurrent.Value <= 0) AniamtionState.Value = AnimationState.Death;
        }

        public void OnAttack()
        {
            IsAttacking.Value = true;

            AniamtionState.Value = AnimationState.Idle;
            Direction.Value = Vector2.zero;

            AniamtionState.Value = AnimationState.Attack;
        }

        public async void DamageUser()
        {
            AniamtionState.Value = AnimationState.Idle;
            _mainGamePlaySystem.UserRecieveDamageHandler.SubstractHp(Damage.Value);

            await UniTask.Delay(1000);

            IsAttacking.Value = false;
        }
    }
}