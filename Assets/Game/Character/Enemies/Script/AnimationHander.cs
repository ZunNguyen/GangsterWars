using Sources.GamePlaySystem.MainGamePlay.Enemies;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Sources.SpawnerSystem;

namespace Game.Character.Enemy
{
    public class AnimationHander : MonoBehaviour
    {
        private IDisposable _disposableAniamtionState;
        private EnemyHandler _enemyHandler;

        [SerializeField] private Animator _animator;

        public void OnSetUp(EnemyHandler enemyHandler, Action onDeath)
        {
            _enemyHandler = enemyHandler;

            _disposableAniamtionState = enemyHandler.AniamtionState.Subscribe(value =>
            {
                var state = value.ConvertToString();
                _animator.SetTrigger(state);

                if (value == Sources.GamePlaySystem.MainGamePlay.Enemies.AnimationState.Death)
                {
                    onDeath?.Invoke();
                }
            });
        }

        public void AttackEnd()
        {
            _enemyHandler.DamageUser();
        }

        public void OnDisposable()
        {
            _disposableAniamtionState?.Dispose();
        }
    }
}