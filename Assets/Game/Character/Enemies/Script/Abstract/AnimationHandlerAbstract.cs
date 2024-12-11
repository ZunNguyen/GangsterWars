using Sources.GamePlaySystem.GameResult;
using Sources.GamePlaySystem.MainGamePlay.Enemies;
using Sources.Utils.Singleton;
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

                if (value == Sources.GamePlaySystem.MainGamePlay.Enemies.AnimationState.Death) OnDisposable();
            }).AddTo(this);
        }

        public abstract void OnAttack();

        public void OnDisposable()
        {
            _disposableAniamtionState?.Dispose();
        }
    }
}