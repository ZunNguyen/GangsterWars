using DG.Tweening;
using Sources.GamePlaySystem.MainMenuGame;
using Sources.Utils.Singleton;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Screens.MainMenuScreen
{
    public abstract class OpenCharacterAbstract : MonoBehaviour
    {
        private const float _duration = 0.5f;

        protected OpenCharacterSystem _openCharacterSystem => Locator<OpenCharacterSystem>.Instance;

        protected OpenCharacterHandlerAbstract _openCharacterAbastract;
        protected TabState _tabCurrentState;

        [SerializeField] private Image _imageCharacter;
        [SerializeField] private TMP_Text _fee;
        [SerializeField] private GameObject _boxCharacter;

        [Header("Tab Handler")]
        [SerializeField] private TabHandler _tabHandler;

        private void Awake()
        {
            _boxCharacter.transform.localScale = Vector3.zero;
            _boxCharacter.gameObject.SetActive(true);

            OnSetUp();
        }

        private void OnSetUp()
        {
            SetValue();
            if (_openCharacterAbastract.IsAldreadyOpenCharacter) return;

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
            _boxCharacter.transform.DOScale(Vector3.one, _duration);
        }

        public void OnCloseBoxClicked()
        {
            _boxCharacter.transform.DOScale(Vector3.zero, _duration);
        }

        public void OnUnlockCharacterClicked()
        {
            var result = _openCharacterAbastract.OpenCharacter();

            if (result)
            {
                _imageCharacter.raycastTarget = false;
                OnCloseBoxClicked();
                _imageCharacter.color = Color.white;
            }
        }

        private void OnDestroy()
        {
            _tabHandler.TabStateChange -= AnimationWhenClickTab;
        }
    }
}