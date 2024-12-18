using Cysharp.Threading.Tasks;
using DG.Tweening;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Character.Enemy.FTUE
{
    public class FTUEHpBar : MonoBehaviour
    {
        private float _duration = 0.5f;

        protected float _maxValue;

        [SerializeField] private FTUEEnemyCtrl _enemyCtrl;
        [SerializeField] private Image _firstImage;
        [SerializeField] private Image _secondImage;

        private void Start()
        {
            _maxValue = _enemyCtrl.HPEnemy.Value;
            _enemyCtrl.HPEnemy.Subscribe(value =>
            {
                ChangeValue(value);

                if (value <= 0) gameObject.SetActive(false);
            }).AddTo(this);
        }

        private async void ChangeValue(int value)
        {
            ChangeFillAmount(value, _firstImage, _duration / 4);
            await UniTask.Delay(1000);
            ChangeFillAmount(value, _secondImage, _duration);
        }

        private void ChangeFillAmount(int value, Image image, float duration = 0)
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