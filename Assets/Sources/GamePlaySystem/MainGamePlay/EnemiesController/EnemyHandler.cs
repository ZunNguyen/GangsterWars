using Sources.DataBaseSystem;
using Sources.Utils.Singleton;
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

    public class EnemyHandler
    {
        private MainGamePlaySystem _mainGamePlaySystem => Locator<MainGamePlaySystem>.Instance;

        public string EnemyId { get; private set; }
        public ReactiveProperty<int> HpCurrent { get; } = new ReactiveProperty<int>();
        public ReactiveProperty<int> Damage { get; } = new ReactiveProperty<int>();
        public ReactiveProperty<Vector2> Direction { get; } = new ReactiveProperty<Vector2>(Vector2.zero);
        public ReactiveProperty<AnimationState> AniamtionState { get;} = new ReactiveProperty<AnimationState>(AnimationState.Idle);

        public void OnSetUp(Enemy enemyInfo)
        {
            EnemyId = enemyInfo.EnemyId;
            HpCurrent.Value = enemyInfo.Hp;
            Damage.Value = enemyInfo.Damage;

            OnStart();
        }

        private void OnStart()
        {
            AniamtionState.Value = AnimationState.Walk;
            Direction.Value = Vector2.left;
        }

        public void Stop()
        {
            AniamtionState.Value = AnimationState.Idle;
            Direction.Value = Vector2.zero;
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
            _mainGamePlaySystem._userRecieveDamageHandler.SubstractHp(Damage.Value);
        }
    }
}