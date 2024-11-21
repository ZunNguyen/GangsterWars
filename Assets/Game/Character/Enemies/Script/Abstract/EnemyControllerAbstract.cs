using Cysharp.Threading.Tasks;
using Game.CanvasInGamePlay.Controller;
using Game.Character.Enemy.Abstract;
using Game.Character.Sniper;
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

namespace Game.Character.Enemy.Abstract
{
    public abstract class EnemyControllerAbstract : MonoBehaviour
    {
        private const float _speed = 1f;

        private MainGamePlaySystem _mainGamePlaySystem => Locator<MainGamePlaySystem>.Instance;
        private SpawnerManager _spawnerManager => Locator<SpawnerManager>.Instance;
        private CoinControllerSystem _coinControllerSystem => Locator<CoinControllerSystem>.Instance;

        private Vector2 _direction;
        private IDisposable _disposableDirection;
        private IDisposable _disposableIsAttacking;
        private IDisposable _disposableIsDeath;

        protected bool _isAttacking = false;
        protected EnemyHandler _enemyHandler;

        [SerializeField] private Rigidbody2D _rb;
        [SerializeField] private Transform _hpBarPos;
        [SerializeField] private AnimationHandlerAbstract _animationHander;

        public void OnSetUp(CanvasInGamePlayController canvasInGamePlayController, string enemyId)
        {
            _enemyHandler = _mainGamePlaySystem.EnemiesController.GetAvailableEnemyHandler();
            _enemyHandler.OnSetUp(enemyId);
            canvasInGamePlayController.OnSetUpHpBar(_hpBarPos, _enemyHandler);

            SubcribeValue();
            _animationHander.OnSetUp(_enemyHandler);
        }

        private void SubcribeValue()
        {
            _disposableDirection = _enemyHandler.Direction.Subscribe(value =>
            {
                _direction = value;
            }).AddTo(this);

            _disposableIsAttacking = _enemyHandler.IsAttacking.Subscribe(value =>
            {
                _isAttacking = value;
            }).AddTo(this);

            _disposableIsDeath = _enemyHandler.AniamtionState.Subscribe(value =>
            {
                if (value == Sources.GamePlaySystem.MainGamePlay.Enemies.AnimationState.Death)
                {
                    OnDeath();
                }
            }).AddTo(this);
        }

        private void FixedUpdate()
        {
            _rb.velocity = _direction * _speed;
        }

        protected virtual void OnTriggerEnter2D(Collider2D collision)
        {
            OnTriggerToStop(collision);
            
            if (collision.tag == CollisionTagKey.BULLET_LEADER)
            {
                var bullet = collision.GetComponent<LeaderWeapon>();
                bullet.ReleaseBullet();
                _enemyHandler.SubstractHp(bullet.Damage, bullet.CollisionKey);
            }

            if (collision.tag == CollisionTagKey.BOM_BOMBER)
            {
                var boom = collision.GetComponent<Bomber.BomberWeapon>();
                _enemyHandler.SubstractHp(boom.Damage, CollisionTagKey.ENEMY_BODY);
            }

            if (collision.tag == CollisionTagKey.BULLET_SNIPER)
            {
                var bullet = collision.GetComponent<SniperWeapon>();
                _enemyHandler.SubstractHp(bullet.Damage, bullet.CollisionKey);
            }
        }

        protected abstract void OnTriggerToStop(Collider2D collision);


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
            _disposableIsAttacking?.Dispose();
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