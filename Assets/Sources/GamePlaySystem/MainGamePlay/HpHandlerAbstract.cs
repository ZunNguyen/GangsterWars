using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using Cysharp.Threading.Tasks;
using Sources.Utils.Singleton;

namespace Sources.GamePlaySystem.MainGamePlay
{
    public abstract class HpHandlerAbstract : MonoBehaviour
    {
        private const float _duration = 0.5f;

        protected MainGamePlaySystem _mainGamePlaySystem => Locator<MainGamePlaySystem>.Instance;

        private float _valueCurrent;
        protected float _maxValue;

        [SerializeField] private Image _firstImage;
        [SerializeField] private Image _secondImage;

        public abstract void OnSetUp();

        protected async void ChangeValue(int value)
        {
            _valueCurrent = value;

            ChangeFillAmount(_firstImage);
            await UniTask.Delay(800);
            ChangeFillAmount(_secondImage);
        }

        private void ChangeFillAmount(Image image)
        {
            DOTween.To(() =>
                            image.fillAmount,
                            x => image.fillAmount = x,
                            _valueCurrent / _maxValue,
                            _duration
                      ).SetEase(Ease.OutQuart);
        }
    }
}