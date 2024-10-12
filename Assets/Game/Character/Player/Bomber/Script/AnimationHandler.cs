using Sources.GamePlaySystem.Bomber;
using Sources.Utils.Singleton;
using UnityEngine;
using UniRx;
using Sources.Extension;

namespace Game.Character.Bomber
{
    public class AnimationHandler : MonoBehaviour
    {
        private BomberSystem _bomberSystem => Locator<BomberSystem>.Instance;

        [SerializeField] private Animator _animator;

        [Header("Controller")]
        [SerializeField] private BomberController _controller;

        private void Awake()
        {
            _bomberSystem.ReloadTimeHandler.TimeReloadCurrent.Subscribe(value =>
            {
                _animator.SetFloat(BomberKey.AnimationKey_Reloading, value);
            }).AddTo(this);
        }

        public void AnimtionThrowing()
        {
            _animator.SetTrigger(BomberKey.AnimationKey_Throwing);
        }

        public void Throwing()
        {
            _controller.ActionHandler.Throwing();
        }
    }
}