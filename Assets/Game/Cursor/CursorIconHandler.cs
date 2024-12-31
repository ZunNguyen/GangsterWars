﻿using Game.Character.Enemy.Abstract;
using Game.Character.Leader;
using Sources.GamePlaySystem.Joystick;
using Sources.GamePlaySystem.Leader;
using Sources.Utils.Singleton;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.Cursor
{
    public class CursorIconHandler : MonoBehaviour
    {
        private const float _radiusRaycast = 0.3f;

        private float _speed = 5f;
        private Vector3 moveDirection;

        private JoystickSystem _joystickSystem => Locator<JoystickSystem>.Instance;
        private LeaderSystem _leaderSystem => Locator<LeaderSystem>.Instance;

        public string NameObjectShoot { get; private set; }

        [SerializeField] private Joystick _joystick;
        [SerializeField] private SpriteRenderer _spriteRenderer;

        private void Awake()
        {
            if (!_joystickSystem.IsUseJoystick) gameObject.SetActive(false);
        }

        private void FixedUpdate()
        {
            float horizontal = _joystick.Horizontal;
            float vertical = _joystick.Vertical;

            moveDirection = new Vector3(horizontal, vertical, 0f);

            transform.position += moveDirection * _speed * Time.deltaTime;
        }

        public void CursorClick()
        {
            SetPosShoot();
            SetNameObjectUserShoot();
            LeaderAction.Instance.LeaderShooting();
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