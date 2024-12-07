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
        private float _duration = 0.5f;

        protected MainGamePlaySystem _mainGamePlaySystem => Locator<MainGamePlaySystem>.Instance;

        protected float _maxValue;

        [SerializeField] private Image _firstImage;
        [SerializeField] private Image _secondImage;

        public abstract void OnSetUp();

        protected async void ChangeValue(float value)
        {
            ChangeFillAmount(value, _firstImage, _duration/4);
            await UniTask.Delay(1000);
            ChangeFillAmount(value, _secondImage, _duration);
        }

        private void ChangeFillAmount(float value, Image image, float duration = 0)
        {
            DOTween.To(() =>
                            image.fillAmount,
                            x => image.fillAmount = x,
                            value / _maxValue,
                            _duration
                      ).SetEase(Ease.OutQuart);
        }
    }
}