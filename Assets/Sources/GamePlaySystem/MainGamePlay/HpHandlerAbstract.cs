using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

namespace Sources.GamePlaySystem.MainGamePlay
{
    public abstract class HpHandlerAbstract : MonoBehaviour
    {
        private float _duration = 5f;

        protected float _maxValue;

        [SerializeField] private Image _firstImage;
        [SerializeField] private Image _secondImage;

        protected abstract void OnSetUp();

        protected void OnChangeFillAmount(float value, Image image, float duration)
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