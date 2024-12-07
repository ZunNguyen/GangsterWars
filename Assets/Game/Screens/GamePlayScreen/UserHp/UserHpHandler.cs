using Cysharp.Threading.Tasks;
using DG.Tweening;
using Sources.GamePlaySystem.MainGamePlay;
using Sources.Utils.Singleton;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Screens.GamePlayScreen
{
    public class UserHpHandler : MonoBehaviour
    {
        private const float _duration = 0.5f;

        private MainGamePlaySystem _mainGamePlaySystem => Locator<MainGamePlaySystem>.Instance;

        [SerializeField] private Slider _slider;
        [SerializeField] private Image _fillWhite;

        private void Awake()
        {
            _slider.onValueChanged.AddListener(OnSliderValueChange);
        }

        public void OnSetUp()
        {
            _slider.maxValue = _mainGamePlaySystem.UserRecieveDamageHandler.HpCurrentUser.Value;

            _mainGamePlaySystem.UserRecieveDamageHandler.HpCurrentUser.Subscribe(value =>
            {
                _slider.value = value;
            }).AddTo(this);
        }

        private async void OnSliderValueChange(float value)
        {
            await UniTask.Delay(1000);

            DOTween.To(() =>
                    _fillWhite.fillAmount,
                    x => _fillWhite.fillAmount = x,
                    value / _slider.maxValue,
                    _duration
                ).SetEase(Ease.OutQuart);
        }

        private void OnDestroy()
        {
            _slider.onValueChanged.RemoveListener(OnSliderValueChange);
        }
    }
}