using Cysharp.Threading.Tasks;
using Game.Character.Enemy.Abstract;
using Game.Character.Leader;
using Sources.GamePlaySystem.Joystick;
using Sources.GamePlaySystem.Leader;
using Sources.Utils.Singleton;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UniRx;
using DG.Tweening;

namespace Game.Cursor
{
    public class CursorIconHandler : MonoBehaviour
    {
        private readonly Vector3 _targetScale = new Vector3(0.15f, 0.15f, 0.15f);
        private const float _duration = 0.1f;
        private const float _radiusRaycast = 0.5f;

        private float _speed = 5f;
        private Vector3 _moveDirection;
        private Vector3 _originalScale;

        private JoystickSystem _joystickSystem => Locator<JoystickSystem>.Instance;
        private LeaderSystem _leaderSystem => Locator<LeaderSystem>.Instance;

        public string NameObjectShoot { get; private set; }

        [SerializeField] private Joystick _joystick;
        [SerializeField] private SpriteRenderer _spriteRenderer;

        private void Awake()
        {
            if (!_joystickSystem.IsUseJoystick)
            {
                gameObject.SetActive(false);
                return;
            }

            _joystickSystem.CursorSensitivity.Subscribe(value =>
            {
                _speed = value;
            }).AddTo(this);

            _originalScale = transform.localScale;
        }

        private void FixedUpdate()
        {
            float horizontal = _joystick.Horizontal;
            float vertical = _joystick.Vertical;

            _moveDirection = new Vector3(horizontal, vertical, 0f);

            Vector3 newPosition = transform.position + _moveDirection * _speed * Time.deltaTime;

            Vector3 clampedPosition = ClampToScreen(newPosition);

            transform.position = clampedPosition;
        }

        private Vector3 ClampToScreen(Vector3 position)
        {
            Vector3 viewportPosition = Camera.main.WorldToViewportPoint(position);

            // viewportPosition(0, 0) is the lower left corner of the screen
            // viewportPosition(1, 1) is the higher right corner of the screen
            viewportPosition.x = Mathf.Clamp(viewportPosition.x, 0.2f, 0.95f);
            viewportPosition.y = Mathf.Clamp(viewportPosition.y, 0.1f, 0.8f);

            return Camera.main.ViewportToWorldPoint(viewportPosition);
        }

        public void CursorClick()
        {
            SetPosShoot();
            SetNameObjectUserShoot();
            LeaderAction.Instance.LeaderShooting();
            Effect();
        }

        private void Effect()
        {
            transform.DOScale(_targetScale, _duration).OnComplete(() =>
            {
                transform.DOScale(_originalScale, _duration);
            });
        }

        private void SetNameObjectUserShoot()
        {
            Vector3 origin = _spriteRenderer.transform.position;
            RaycastHit2D[] raycastHits = Physics2D.CircleCastAll(origin, _radiusRaycast, Vector2.zero);

            LeaderAction.Instance.SetNameObjectUserShoot(raycastHits);
        }

        private void SetPosShoot()
        {
            LeaderAction.Instance.SetPosShoot(transform.position);
        }
    }
}