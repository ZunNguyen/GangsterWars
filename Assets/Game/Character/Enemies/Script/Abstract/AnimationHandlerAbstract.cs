using Sources.GamePlaySystem.MainGamePlay.Enemies;
using System;
using UniRx;
using UnityEngine;

namespace Game.Character.Enemy.Abstract
{
    public abstract class AnimationHandlerAbstract : MonoBehaviour
    {
        private IDisposable _disposableAniamtionState;
        
        protected EnemyHandler _enemyHandler;

        [SerializeField] private Animator _animator;

        public virtual void OnSetUp(EnemyHandler enemyHandler)
        {
            _enemyHandler = enemyHandler;

            _disposableAniamtionState = enemyHandler.AniamtionState.Subscribe(value =>
            {
                var state = value.ConvertToString();
                _animator.SetTrigger(state);
            }).AddTo(this);
        }

        public abstract void OnAttackEnd();

        public void OnDisposable()
        {
            _disposableAniamtionState?.Dispose();
        }
    }
}