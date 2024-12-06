using Sources.GamePlaySystem.CoinController;
using Sources.Utils.Singleton;
using TMPro;
using UnityEngine;
using UniRx;
using Sources.Utils;
using System.Threading;
using Sources.SpawnerSystem;
using DG.Tweening;

namespace Game.Screens.MainMenuScreen
{
    public class CoinHandler : MonoBehaviour
    {
        private readonly Vector3 _targetScale = new Vector3(1.1f, 1.1f, 1.1f);
        private const float _duration = 0.2f;

        private CoinControllerSystem _coinControllerSystem => Locator<CoinControllerSystem>.Instance;
        private SpawnerManager _spawnerManager => Locator<SpawnerManager>.Instance;

        [SerializeField] private TMP_Text _text;
        [SerializeField] private CoinText _coinText;

        private void Awake()
        {
            _coinControllerSystem.Coins.Subscribe(value =>
            {
                _text.text = ShortNumber.Get(value);
            }).AddTo(this);

            _coinControllerSystem.OnAddCoin += AnimationAdd;
            _coinControllerSystem.OnSubstractCoin += AnimationSubstract;
        }

        private void AnimationAdd(int quality)
        {
            var coinText = GetCoinText();
            coinText.OnSetUp(quality, true);
            AnimationCoinText();
        }

        private void AnimationSubstract(int quality)
        {
            var coinText = GetCoinText();
            coinText.OnSetUp(quality, false);
            AnimationCoinText();
        }

        private void AnimationCoinText()
        {
            _text.transform.DOScale(_targetScale, _duration).SetEase(Ease.InOutSine).OnComplete( () =>
            {
                _text.transform.DOScale(Vector3.one, _duration).SetEase(Ease.InOutSine);
            });
        }

        private CoinText GetCoinText()
        {
            var coinText = _spawnerManager.Get(_coinText);
            coinText.transform.SetParent(transform, false);
            coinText.transform.position = _text.transform.position;

            return coinText;
        }

        private void OnDestroy()
        {
            _coinControllerSystem.OnAddCoin -= AnimationAdd;
            _coinControllerSystem.OnSubstractCoin -= AnimationSubstract;
        }
    }
}

