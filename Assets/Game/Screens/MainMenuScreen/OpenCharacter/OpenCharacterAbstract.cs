using Cysharp.Threading.Tasks;
using DG.Tweening;
using Sources.Audio;
using Sources.Extension;
using Sources.GamePlaySystem.MainMenuGame;
using Sources.Utils;
using Sources.Utils.Singleton;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

namespace Game.Screens.MainMenuScreen
{
    public abstract class OpenCharacterAbstract : MonoBehaviour
    {
        private readonly Vector3 _targetScale = new Vector3(1.2f, 1.2f, 1.2f);
        private const float _offsetMoveY = 1f;
        private const float _duration = 0.5f;

        private AudioManager _audioManager => Locator<AudioManager>.Instance;
        protected OpenCharacterSystem _openCharacterSystem => Locator<OpenCharacterSystem>.Instance;
        private StoreSystem _storeSystem => Locator<StoreSystem>.Instance;

        private Tween _tween;
        private float _posOriginalY;
        private float _targetMoveY;
        private bool _isAldreadyOpen;

        protected OpenCharacterHandlerAbstract _openCharacterAbastract;
        protected TabState _tabCurrentState;

        [SerializeField] private Image _imageCharacter;
        [SerializeField] private TMP_Text _fee;
        [SerializeField] private RectTransform _unlockMe;
        [SerializeField] private GameObject _boxCharacter;
        [SerializeField] private GameObject _blackBG;

        [Header("Tab Handler")]
        [SerializeField] private TabHandler _tabHandler;

        private async void Awake()
        {
            _boxCharacter.transform.localScale = Vector3.zero;
            _boxCharacter.gameObject.SetActive(true);
            _posOriginalY = transform.position.y;
            _targetMoveY = transform.position.y + _offsetMoveY;

            await UniTask.DelayFrame(5);
            OnSetUp();
        }

        private void OnSetUp()
        {
            SetValue();
            _isAldreadyOpen = _openCharacterAbastract.IsAldreadyOpenCharacter;
            if (_isAldreadyOpen)
            {
                _imageCharacter.raycastTarget = false;
                return;
            }

            _imageCharacter.color = Color.black;
            _unlockMe.gameObject.SetActive(true);
            _fee.text = ShortNumber.Get(_openCharacterAbastract.CharacterFee);

            AnimationUnlockMe();
        }

        protected abstract void SetValue();

        private void AnimationUnlockMe()
        {
            _tween = _unlockMe.transform.DOScale(_targetScale, _duration).SetEase(Ease.InOutSine).OnComplete(() =>
            {
                _unlockMe.transform.DOScale(Vector3.one, _duration).SetEase(Ease.InOutSine);
            }).SetLoops(-1, LoopType.Yoyo);
        }

        public void OnOpenBoxClicked()
        {
            if (_isAldreadyOpen) return;
            _audioManager.Play(AudioKey.SFX_CLICK_01);
            _blackBG.SetActive(true);
            _unlockMe.gameObject.SetActive(false);
            _boxCharacter.transform.DOScale(Vector3.one, _duration);
        }

        public void OnCloseBoxClicked()
        {
            _audioManager.Play(AudioKey.SFX_CLICK_01);
            _blackBG.SetActive(false);
            _unlockMe.gameObject.SetActive(true);
            _boxCharacter.transform.DOScale(Vector3.zero, _duration);
        }

        public void OnUnlockCharacterClicked()
        {
            var result = _openCharacterAbastract.OpenCharacter();

            if (result)
            {
                _audioManager.Play(AudioKey.SFX_CLICK_01);
                _imageCharacter.raycastTarget = false;
                _unlockMe.gameObject.SetActive(false);
                OnCloseBoxClicked();
                _imageCharacter.color = Color.white;
                _tween?.Kill();
                _isAldreadyOpen = true;
            }
            else _audioManager.Play(AudioKey.SFX_CLICK_ERROR);
        }
    }
}