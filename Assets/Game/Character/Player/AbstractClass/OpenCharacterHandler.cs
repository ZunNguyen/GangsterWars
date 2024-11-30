using Sources.GamePlaySystem.MainMenuGame;
using Sources.Utils.Singleton;
using UnityEngine;

namespace Game.Character.Player.Abstract
{
    public abstract class OpenCharacterHandler : MonoBehaviour
    {
        protected OpenCharacterSystem _openCharacterSystem => Locator<OpenCharacterSystem>.Instance;

        private OpenCharacterHandlerAbstract _openCharacterHandler;

        private Transform _transformPos;
        private Camera _camera;

        [SerializeField] private RectTransform _rectTransform;

        protected virtual void OnSetUp(OpenCharacterHandlerAbstract openCharacterHandler)
        {
            _openCharacterHandler = openCharacterHandler;

            gameObject.SetActive(!_openCharacterHandler.IsAldreadyOpenCharacter);
            _transformPos = _openCharacterHandler.CharacterPos;
            _camera = _openCharacterHandler.Camera;

            SetPos();
        }

        private void SetPos()
        {
            Vector3 screenPosition = _camera.WorldToScreenPoint(_transformPos.position);

            Vector2 localPosition = new Vector2(
                screenPosition.x - (Screen.width / 2f),
                screenPosition.y - (Screen.height / 2f)
            );

            localPosition /= _rectTransform.lossyScale;

            _rectTransform.anchoredPosition = localPosition;
        }
    }
}