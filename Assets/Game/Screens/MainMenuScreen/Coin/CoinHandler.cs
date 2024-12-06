using Sources.GamePlaySystem.CoinController;
using Sources.Utils.Singleton;
using TMPro;
using UnityEngine;
using UniRx;
using Sources.Utils;
using System.Threading;
using Sources.SpawnerSystem;

namespace Game.Screens.MainMenuScreen
{
    public class CoinHandler : MonoBehaviour
    {
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
        }

        private void AnimationSubstract(int quality)
        {
            var coinText = GetCoinText();
            coinText.OnSetUp(quality, false);
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

