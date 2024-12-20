using Cysharp.Threading.Tasks;
using DG.Tweening;
using Sources.Audio;
using Sources.Extension;
using Sources.GamePlaySystem.MainMenuGame;
using Sources.Utils.Singleton;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Screens.MainMenuScreen
{
    public class TabCoin : MonoBehaviour
    {
        private const float _duration = 0.7f;
        private const float _doFade = 0.7f;

        private StoreSystem _storeSystem => Locator<StoreSystem>.Instance;
        private AudioManager _audioManager => Locator<AudioManager>.Instance;

        private bool _isOpened = false;
        private TabState _tabStateCurrent;
        private Tween _tween;

        [SerializeField] private GameObject _notSelected;
        [SerializeField] private Image _mark;

        private void Awake()
        {
            _tabStateCurrent = TabState.TabCoin;

            AnimationMark();

            _storeSystem.StoreEarnCoinHandler.IsAnyPackToEarn.Subscribe(value =>
            {
                if (value) _tween.Play();
                else _tween.Pause();
                _mark.gameObject.SetActive(value);
            }).AddTo(this);
        }

        private void AnimationMark()
        {
            _tween = _mark.DOFade(_doFade, _duration).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
        }

        public void OnClicked()
        {
            _audioManager.Play(AudioKey.SFX_CLICK_01);
            _storeSystem.SetTabCurrent(_tabStateCurrent);
        }
    }
}