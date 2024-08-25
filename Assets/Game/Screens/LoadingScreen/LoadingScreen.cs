using UnityEngine;
using Sources.UI;
using UnityEngine.UI;
using UniRx;

namespace Game.Screens.LoadingScreen
{
    public class LoadingScreen : BaseUI
    {
        [SerializeField] private Slider _slider;

        private void Awake()
        {
            _slider.value = 0;
        }

        public override void OnSetUp(object paramater = null)
        {
            base.OnSetUp(paramater);

            var progress = paramater as FloatReactiveProperty;
            progress.Subscribe(UpdateLoadingProgress).AddTo(this);
        }

        private void UpdateLoadingProgress(float value)
        {
            _slider.value = value;
        }
    }
}