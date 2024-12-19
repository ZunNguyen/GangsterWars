using Cysharp.Threading.Tasks;
using DG.Tweening;
using Sources.DataBaseSystem;
using Sources.GamePlaySystem.MainMenuGame;
using Sources.SpawnerSystem;
using Sources.Utils.Singleton;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Screens.MainMenuScreen
{
    public class BuyCoinView : MonoBehaviour
    {
        private const float _duration = 0.4f;

        private SpawnerManager _spawnerManager => Locator<SpawnerManager>.Instance;
        private StoreSystem _storeSystem => Locator<StoreSystem>.Instance;

        private string _packId;
        private RectTransform _posCoinTotal;

        [Header("Coin icon")]
        [SerializeField] private GameObject _iconCoin;

        [Header("Box Coin")]
        [SerializeField] private TMP_Text _textCoin;
        [SerializeField] private Image _iconBox;

        public void OnSetUp(EarnCoinInfo buyCoinInfo, RectTransform posCoinTotal)
        {
            _packId = buyCoinInfo.Id;
            _iconBox.sprite = buyCoinInfo.Sprite;
            _textCoin.text = buyCoinInfo.Value.ToString();
            _posCoinTotal = posCoinTotal;
        }

        public async void OnClaimClicked()
        {
            var result = await _storeSystem.StoreBuyCoinHandler.ShowAdCoin(_packId);

            if (result)
            {
                await UniTask.Delay(500);
                AnimationCollectCoin();
            }
        }

        private async void AnimationCollectCoin()
        {
            var rect = new RectTransform();
            var delay = (int)(_duration / 2 * 1000);

            for (int i = 0; i < _storeSystem.StoreBuyCoinHandler._countCoinIcon; i++)
            {
                var newIcon = _spawnerManager.Get(_iconCoin);
                newIcon.SetActive(true);
                rect = newIcon.GetComponent<RectTransform>();

                rect.transform.SetParent(_posCoinTotal, false);
                rect.position = transform.position;

                rect.DOMove(_posCoinTotal.position, _duration).SetEase(Ease.InOutSine).OnComplete(() =>
                {
                    _storeSystem.StoreBuyCoinHandler.AddCoin();
                    _spawnerManager.Release(newIcon);
                });
                await UniTask.Delay(delay);
            }
        }
    }
}