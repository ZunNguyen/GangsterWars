﻿using Cysharp.Threading.Tasks;
using DG.Tweening;
using Sources.Extension;
using Sources.GamePlaySystem.Leader;
using Sources.SpawnerSystem;
using Sources.Utils.Singleton;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Game.Weapon.Bullet
{
    public class BulletWeapon : MonoBehaviour
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
            Damage = _leaderSystem.GunHandler.DamageBulletCurrent;
        }

        public void MoveMent(Vector3 clickMousePos)
        {
            Vector2 direction = (clickMousePos - transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

            var duration = Vector3.Distance(clickMousePos, _originalPos) / _speed;

            transform.DOMove(clickMousePos, duration).SetEase(Ease.InSine)
                .OnComplete(() =>
                {
                    Debug.Log($"Bullet pos: {transform.position}");
                    ReleaseBullet();
                });
        }

        public void ReleaseBullet()
        {
            if (this.isActiveAndEnabled == false) return;
            _spawnerManager.Release<BulletWeapon>(this);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag == CollisionTagKey.HEAD) CollisionKey = CollisionTagKey.HEAD;
            if (collision.tag == CollisionTagKey.BODY) CollisionKey = CollisionTagKey.BODY;
        }
    }
}