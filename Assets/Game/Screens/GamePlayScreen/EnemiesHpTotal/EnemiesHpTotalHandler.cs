using Sources.GamePlaySystem.MainGamePlay;
using Sources.Utils.Singleton;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using Cysharp.Threading.Tasks;
using Unity.VisualScripting;
using DG.Tweening;

namespace Game.Screens.GamePlayScreen
{
    public class EnemiesHpTotalHandler : MonoBehaviour
    {
        private const float _duration = 0.5f;

        private MainGamePlaySystem _mainGamePlaySystem => Locator<MainGamePlaySystem>.Instance;

        private float trailingValue;

        [SerializeField] private Slider _slider;
        [SerializeField] private Image _fillWhite;

        private void Awake()
        {
            _slider.onValueChanged.AddListener(OnSliderValueChange);
        }

        public async void OnSetUp()
        {
            await UniTask.Delay(1000);
            _slider.maxValue = _slider.value = _mainGamePlaySystem.EnemiesController.TotalHpEnemies;

            _mainGamePlaySystem.EnemiesController.HpEnemiesCurrent.Subscribe(value =>
            {
                _slider.value = value;
            }).AddTo(this);

            trailingValue = _slider.value;
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