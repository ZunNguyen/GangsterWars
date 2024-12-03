using Cysharp.Threading.Tasks;
using DG.Tweening;
using Sources.Audio;
using Sources.Extension;
using Sources.GamePlaySystem.MainMenuGame;
using Sources.Utils.Singleton;
using TMPro;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Screens.MainMenuScreen
{
    public abstract class OpenCharacterAbstract : MonoBehaviour
    {
        private const float _duration = 0.5f;

        private AudioManager _audioManager => Locator<AudioManager>.Instance;
        protected OpenCharacterSystem _openCharacterSystem => Locator<OpenCharacterSystem>.Instance;

        protected OpenCharacterHandlerAbstract _openCharacterAbastract;
        protected TabState _tabCurrentState;

        [SerializeField] private Image _imageCharacter;
        [SerializeField] private TMP_Text _fee;
        [SerializeField] private GameObject _boxCharacter;

        [Header("Tab Handler")]
        [SerializeField] private TabHandler _tabHandler;

        private async void Awake()
        {
            _boxCharacter.transform.localScale = Vector3.zero;
            _boxCharacter.gameObject.SetActive(true);

            await UniTask.Delay(100);
            OnSetUp();
        }

        private void OnSetUp()
        {
            SetValue();
            if (_openCharacterAbastract.IsAldreadyOpenCharacter)
            {
                _imageCharacter.raycastTarget = false;
                return;
            }

            _imageCharacter.color = Color.black;
            _fee.text = _openCharacterAbastract.CharacterFee.ToString();

            _tabHandler.TabStateChange += AnimationWhenClickTab;
        }

        protected abstract void SetValue();

        private void AnimationWhenClickTab(TabState tabState)
        {
            if (_tabCurrentState != tabState) return;
            
            // Animation
        }

        public void OnOpenBoxClicked()
        {
            _audioManager.Play(AudioKey.SFX_CLICK_01);
            _boxCharacter.transform.DOScale(Vector3.one, _duration);
        }

        public void OnCloseBoxClicked()
        {
            _audioManager.Play(AudioKey.SFX_CLICK_01);
            _boxCharacter.transform.DOScale(Vector3.zero, _duration);
        }

        public void OnUnlockCharacterClicked()
        {
            var result = _openCharacterAbastract.OpenCharacter();

            if (result)
            {
                _audioManager.Play(AudioKey.SFX_CLICK_01);
                _imageCharacter.raycastTarget = false;
                OnCloseBoxClicked();
                _imageCharacter.color = Color.white;
            }
            else _audioManager.Play(AudioKey.SFX_CLICK_ERROR);
        }

        private void OnDestroy()
        {
            _tabHandler.TabStateChange -= AnimationWhenClickTab;
        }
    }
}