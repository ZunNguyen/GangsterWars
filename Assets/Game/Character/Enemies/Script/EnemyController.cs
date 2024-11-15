using Cysharp.Threading.Tasks;
using Game.CanvasInGamePlay.Controller;
using Game.Weapon.Bullet;
using Sources.Extension;
using Sources.GamePlaySystem.CoinController;
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
        private CoinControllerSystem _coinControllerSystem => Locator<CoinControllerSystem>.Instance;

        private EnemyHandler _enemyHandler;
        private Vector2 _direction;
        private bool _isAttacking = false;

        private IDisposable _disposableDirection;
        private IDisposable _disposableIsAttacking;

        [SerializeField] private Rigidbody2D _rb;
        [SerializeField] private Animator _animator;
        [SerializeField] private Transform _hpBarPos;
        [SerializeField] private AnimationHander _animationHander;

        public void OnSetUp(CanvasInGamePlayController canvasInGamePlayController, string enemyId)
        {
            _enemyHandler = _mainGamePlaySystem.EnemiesController.GetAvailableEnemyHandler();
            _enemyHandler.OnSetUp(enemyId);
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
            if (collision.tag == CollisionTagKey.SHIELD && !_isAttacking)
            {
                _enemyHandler.OnAttack();
            }

            if (collision.tag == CollisionTagKey.BULLET)
            {
                var bulletCtrl = collision.GetComponent<BulletWeapon>();
                bulletCtrl.ReleaseBullet();
                _enemyHandler.SubstractHp(bulletCtrl.Damage, bulletCtrl.CollisionKey);
            }

            if (collision.tag == CollisionTagKey.BOOM)
            {
                var boom = collision.GetComponent<Bomber.BomberWeapon>();
                _enemyHandler.SubstractHp(boom.Damage, CollisionTagKey.BODY);
            }
        }

        private void OnDeath()
        {
            var coinRewardInfo = new CoinRewardInfo()
            {
                PosSpawn = transform,
                Coins = _enemyHandler.CoinsReward,
            };

            _coinControllerSystem.SpawnCoinReward(coinRewardInfo);
            OnDisposable();
            AnimationDeath();
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