using Cysharp.Threading.Tasks;
using Sources.SpawnerSystem;
using Sources.Utils.Singleton;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Game.Weapon.Bullet
{
    public class BulletMoveMent : MonoBehaviour
    {
        private SpawnerManager _spawnerManager => Locator<SpawnerManager>.Instance;

        private Vector2 _originalPos;

        [SerializeField] private Rigidbody2D _rb;
        [SerializeField] private float distance;

        private void Awake()
        {
            _originalPos = transform.position;
        }

        public async UniTask MoveMent(Vector3 clickMousePos)
        {
            var direction = (clickMousePos - transform.position).normalized;
            _rb.velocity = direction * 10f;

            while (true)
            {
                distance = Math.Abs(Vector2.Distance(_originalPos, _rb.position));

                if (distance > 15f)
                {
                    _spawnerManager.Release<BulletMoveMent>(this);
                    break;
                }

                await UniTask.Yield(PlayerLoopTiming.Update);
            }
        }
    }
}