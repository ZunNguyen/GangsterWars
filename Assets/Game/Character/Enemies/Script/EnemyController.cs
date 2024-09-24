using Game.Weapon.Bullet;
using Sources.DataBaseSystem;
using Sources.Extension;
using Sources.GamePlaySystem.MainGamePlay;
using Sources.GamePlaySystem.MainGamePlay.Enemies;
using Sources.SpawnerSystem;
using Sources.Utils.Singleton;
using System;
using UniRx;
using UnityEngine;

namespace Game.Character.Enemy
{
    public class EnemyController : MonoBehaviour
    {
        private MainGamePlaySystem _mainGamePlaySystem => Locator<MainGamePlaySystem>.Instance;
        private SpawnerManager _spawnerManager => Locator<SpawnerManager>.Instance;

        private EnemyHandler _enemyHandler;
        private Vector2 _direction;

        private IDisposable _disposableDirection;
        private IDisposable _disposableAniamtionState;

        [SerializeField] private Rigidbody2D _rb;

        public void OnSetUp(Sources.DataBaseSystem.Enemy enemy)
        {
            _enemyHandler = _mainGamePlaySystem.EnemiesController.GetAvailableEnemyHandler();
            _enemyHandler.OnSetUp(enemy);

            SubcribeDirection();
            SubcribeAniamtionState();
        }

        private void SubcribeDirection()
        {
            _disposableDirection = _enemyHandler.Direction.Subscribe(value =>
            {
                _direction = value;
            });
        }

        private void SubcribeAniamtionState()
        {
            _disposableAniamtionState = _enemyHandler.AniamtionState.Subscribe(value =>
            {
                if (value == Sources.GamePlaySystem.MainGamePlay.Enemies.AnimationState.Death)
                {
                    OnDeath();
                }
            });
        }

        private void FixedUpdate()
        {
            _rb.velocity = _direction * 1f;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag == CollisionTagKey.Shield)
            {
                _enemyHandler.Stop();
            }

            if (collision.tag == CollisionTagKey.Bullet)
            {
                var damage = collision.GetComponent<BulletController>().Damage;
                _enemyHandler.SubstractHp(damage);
            }
        }

        private void OnDeath()
        {
            _spawnerManager.Release<EnemyController>(this);
            Debug.Log($"Enemy death");

            OnDisposable();
        }

        private void OnDisposable()
        {
            _disposableDirection?.Dispose();
            _disposableAniamtionState?.Dispose();
        }
    }
}