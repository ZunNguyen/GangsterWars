using Cysharp.Threading.Tasks;
using DG.Tweening;
using Sources.DataBaseSystem;
using Sources.GamePlaySystem.MainMenuGame;
using Sources.GamePlaySystem.MainMenuGame.Store;
using Sources.SpawnerSystem;
using Sources.Utils.Singleton;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using Sources.Utils.DateTime;
using System;
using Game.Screens.Coin;

namespace Game.Screens.MainMenuScreen
{
    public class EarnCoinView : MonoBehaviour
    {
        private readonly Vector3 _targetScale = new Vector3(1.1f, 1.1f, 1.1f);
        private const float _durationScale = 0.8f;
        private const int _delayEarnCoin = 200;

        private SpawnerManager _spawnerManager => Locator<SpawnerManager>.Instance;
        private StoreSystem _storeSystem => Locator<StoreSystem>.Instance;

        private bool _isCanClaim = false;
        private float _maxValueAmount;
        private Transform _posCoinTotal;
        private PackEarnCoinViewHandler _handler;
        private Tween _tween;

        [Header("Coin icon")]
        [SerializeField] private Coin.Coin _coinPrefab;

        [Header("Box Coin")]
        [SerializeField] private TMP_Text _textCoin;
        [SerializeField] private Image _iconBox;

        [Header("Shadow")]
        [SerializeField] private GameObject _shadow;
        [SerializeField] private Image _amount;
        [SerializeField] private TMP_Text _textCountTime;

        [Header("Another")]
        [SerializeField] private RectTransform _selfRect;

        public void OnSetUp(EarnCoinInfo buyCoinInfo, Transform posCoinTotal)
        {
            _iconBox.sprite = buyCoinInfo.Sprite;
            _textCoin.text = buyCoinInfo.Value.ToString();
            _posCoinTotal = posCoinTotal;

            _handler = _storeSystem.StoreEarnCoinHandler.GetPackEarnCoinViewHandler(buyCoinInfo.Id);
            _maxValueAmount = _handler.TimeToEarn;

            SetAnimationCanClaim();
            SubscribeTimeRemain();
            SubscribeIsCanClaim();
        }

        private void SubscribeTimeRemain()
        {
            _handler.TimeRemain.Subscribe(value =>
            {
                _textCountTime.text = DateTimeUtils.ChangeSecondToDateTime(value);
                _shadow.SetActive(value != 0);
                _amount.fillAmount = value / _maxValueAmount;
            }).AddTo(this);
        }

        private void SubscribeIsCanClaim()
        {
            _handler.IsCanClaim.Subscribe(value =>
            {
                _isCanClaim = value;
                if (value) _tween.Play();
                else
                {
                    _tween.Pause();
                    _selfRect.localScale = Vector3.one;
                }
            }).AddTo(this);
        }

        private void SetAnimationCanClaim()
        {
            _tween = _selfRect.DOScale(_targetScale, _durationScale)
                .SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
        }

        public async void OnClaimClicked()
        {
            if (!_isCanClaim) return;
            var result = await _storeSystem.StoreEarnCoinHandler.ShowAdCoin();

            if (result)
            {
                await UniTask.Delay(500);
                _handler.ResetTimeToEarn();
                AnimationCollectCoin();
            }
        }

        private async void AnimationCollectCoin()
        {
            for (int i = 0; i < _handler.CountCoinIcon; i++)
            {
                var newIcon = _spawnerManager.Get(_coinPrefab);
                
                newIcon.gameObject.SetActive(true);
                newIcon.transform.SetParent(_posCoinTotal, false);
                newIcon.transform.position = transform.position;
                
                newIcon.OnSetUp(_posCoinTotal, _handler.OnceEarnCoin, 3000);
                await UniTask.Delay(_delayEarnCoin);
            }
        }
    }
}