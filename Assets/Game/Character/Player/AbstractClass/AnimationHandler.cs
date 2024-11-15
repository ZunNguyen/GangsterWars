using Sources.Extension;
using Sources.GamePlaySystem.Character;
using Sources.Utils.Singleton;
using UniRx;
using UnityEngine;

namespace Game.Character.Abstract
{
    public abstract class AnimationHandler : MonoBehaviour
    {
        protected ReloadTimeHandler _reloadTimeHandler;
        protected WeaponHandler _weaponHandler;

        protected string _animationShootKey;
        protected string _animationReloadKey;

        [SerializeField] private Animator _animator;
        [SerializeField] private Abstract.ActionHandler _actionHandler;

        protected abstract void OnSetUp();

        private void Awake()
        {
            OnSetUp();

            _reloadTimeHandler.TimeReloadCurrent.Subscribe(value =>
            {
                _animator.SetFloat(_animationReloadKey, value);
            }).AddTo(this);

            _weaponHandler.Attack += AnimationAttack;
        }

        private void AnimationAttack()
        {
            _weaponHandler.SetIsAnimationComplete(false);
            _animator.SetTrigger(_animationShootKey);
        }

        public void Attack()
        {
            _actionHandler.Throwing();
        }

        public void OnCompleteAnimation()
        {
            _weaponHandler.SetIsAnimationComplete(true);
        }

        private void OnDestroy()
        {
            _weaponHandler.Attack -= AnimationAttack;
        }
    }
}