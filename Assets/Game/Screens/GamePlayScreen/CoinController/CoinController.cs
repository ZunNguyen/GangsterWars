using Cysharp.Threading.Tasks;
using Sources.GamePlaySystem.CoinController;
using Sources.Utils;
using Sources.Utils.Singleton;
using TMPro;
using UniRx;
using UnityEngine;

namespace Game.Screens.GamePlayScreen
{
    public class CoinController : MonoBehaviour
    {
        private CoinControllerSystem _coinControllerSystem => Locator<CoinControllerSystem>.Instance;

        [SerializeField] private TMP_Text _text;

        private void Awake()
        {
            _coinControllerSystem.Coins.Subscribe(value =>
            {
                _text.text = ShortNumber.Get(value);
            }).AddTo(this);
        }
    }
}