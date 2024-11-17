﻿using DG.Tweening;
using Sources.Extension;
using Sources.GamePlaySystem.Leader;
using Sources.SpawnerSystem;
using Sources.Utils.Singleton;
using UnityEngine;

namespace Game.Weapon.Bullet
{
    public class LeaderWeapon : MonoBehaviour
    {
        private const float _speed = 30f;
        private SpawnerManager _spawnerManager => Locator<SpawnerManager>.Instance;
        private LeaderSystem _leaderSystem => Locator<LeaderSystem>.Instance;

        private Vector2 _originalPos;

        public string CollisionKey { get; private set; }
        public int Damage { get; private set; }

        [SerializeField] private Rigidbody2D _rb;

        private void Awake()
        {
            _originalPos = transform.position;
        }

        public void MoveMent(Vector3 clickMousePos)
        {
            Damage = _leaderSystem.GunHandler.DamageBulletCurrent;

            Vector2 direction = (clickMousePos - transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

            var duration = Vector3.Distance(clickMousePos, _originalPos) / _speed;

            transform.DOMove(clickMousePos, duration).SetEase(Ease.InSine)
                .OnComplete(() => ReleaseBullet());
        }

        public void ReleaseBullet()
        {
            if (isActiveAndEnabled == false) return;
            _spawnerManager.Release(this);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag == CollisionTagKey.ENEMY_HEAD) CollisionKey = CollisionTagKey.ENEMY_HEAD;
            if (collision.tag == CollisionTagKey.ENEMY_BODY) CollisionKey = CollisionTagKey.ENEMY_BODY;
        }
    }
}