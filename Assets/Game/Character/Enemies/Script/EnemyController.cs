using Cysharp.Threading.Tasks;
using Game.CanvasInGamePlay.Controller;
using Game.Character.Bomber;
using Game.Weapon.Bullet;
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
        private bool _isAttacking = false;

        private IDisposable _disposableDirection;
        private IDisposable _disposableIsAttacking;

        [SerializeField] private Rigidbody2D _rb;
        [SerializeField] private Animator _animator;
        [SerializeField] private Transform _hpBarPos;
        [SerializeField] private AnimationHander _animationHander;

        public void OnSetUp(Sources.DataBaseSystem.Enemy enemy, CanvasInGamePlayController canvasInGamePlayController)
        {
            _enemyHandler = _mainGamePlaySystem.EnemiesController.GetAvailableEnemyHandler();
            _enemyHandler.OnSetUp(enemy);
            canvasInGamePlayController.OnSetUpHpBar(_hpBarPos, _enemyHandler);

            SubcribeDirection();
            _animationHander.OnSetUp(_enemyHandler, OnDeath);
        }

        private void SubcribeDirection()
        {
            _disposableDirection = _enemyHandler.Direction.Subscribe(value =>
            {
                _direction = value;
            });

            _disposableIsAttacking = _enemyHandler.IsAttacking.Subscribe(value =>
            {
                _isAttacking = value;
            });
        }

        private void FixedUpdate()
        {
            _rb.velocity = _direction * 1f;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag == CollisionTagKey.Shield && !_isAttacking)
            {
                _enemyHandler.OnAttack();
            }

            if (collision.tag == CollisionTagKey.Bullet)
            {
                var bulletCtrl = collision.GetComponent<BulletWeapon>();
                bulletCtrl.ReleaseBullet();
                _enemyHandler.SubstractHp(bulletCtrl.Damage);
            }

            if (collision.tag == CollisionTagKey.Boom)
            {
                var boom = collision.GetComponent<Bomber.Weapon>();
                _enemyHandler.SubstractHp(boom.Damage);
            }
        }

        private void OnDeath()
        {
            Debug.Log($"Enemy death");
            AnimationDeath();
            OnDisposable();
        }

        private void OnDisposable()
        {
            _disposableDirection?.Dispose();
            _animationHander.OnDisposable();
        }

        private async void AnimationDeath()
        {
            await UniTask.Delay(2000);

            _spawnerManager.Release(this);
            _mainGamePlaySystem.SpawnEnemiesHandler.RemoveEnemyToList(this);
        }
    }
}