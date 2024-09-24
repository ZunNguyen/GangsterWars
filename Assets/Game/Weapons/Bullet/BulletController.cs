using Cysharp.Threading.Tasks;
using DG.Tweening;
using Sources.SpawnerSystem;
using Sources.Utils.Singleton;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Game.Weapon.Bullet
{
    public class BulletController : MonoBehaviour
    {
        private const float _speed = 30f;
        private SpawnerManager _spawnerManager => Locator<SpawnerManager>.Instance;

        private Vector2 _originalPos;

        public int Damage {  get; private set; }

        [SerializeField] private Rigidbody2D _rb;

        private void Awake()
        {
            _originalPos = transform.position;
        }

        public void MoveMent(Vector3 clickMousePos)
        {
            Vector2 direction = (clickMousePos - transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

            var duration = Vector3.Distance(clickMousePos, _originalPos) / _speed;

            transform.DOMove(clickMousePos, duration).SetEase(Ease.InSine)
                .OnComplete(() => _spawnerManager.Release<BulletController>(this));
        }
    }
}