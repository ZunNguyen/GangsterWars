using Cysharp.Threading.Tasks;
using DG.Tweening;
using Game.CanvasInGamePlay.Controller;
using Game.Character.Sniper;
using Game.Weapon.Bullet;
using Sources.Extension;
using Sources.GamePlaySystem.CoinController;
using Sources.GamePlaySystem.GameResult;
using Sources.GamePlaySystem.MainGamePlay;
using Sources.GamePlaySystem.MainGamePlay.Enemies;
using Sources.SpawnerSystem;
using Sources.Utils.Singleton;
using System;
using System.Threading;
using UniRx;
using UnityEngine;

namespace Game.Character.Enemy.Abstract
{
    public abstract class EnemyControllerAbstract : MonoBehaviour
    {
        private const float _speed = 0.7f;
        private const float _durationDoFade = 0.3f;

        private MainGamePlaySystem _mainGamePlaySystem => Locator<MainGamePlaySystem>.Instance;
        private SpawnerManager _spawnerManager => Locator<SpawnerManager>.Instance;
        private CoinControllerSystem _coinControllerSystem => Locator<CoinControllerSystem>.Instance;
        private GameResultSystem _gameResultSystem => Locator<GameResultSystem>.Instance;

        private Vector2 _direction;
        private Tween _tween;
        private CancellationToken _token;
        private IDisposable _disposableDirection;
        private IDisposable _disposableIsAttacking;
        private IDisposable _disposableIsDeath;

        protected bool _isAttacking = false;
        protected EnemyHandler _enemyHandler;

        public int IndexPos { get; private set; }

        [SerializeField] private Rigidbody2D _rb;
        [SerializeField] private Transform _hpBarPos;
        [SerializeField] private Transform _damageFeedPos;
        [SerializeField] private AnimationHandlerAbstract _animationHander;
        [SerializeField] private SpriteRenderer _spriteRenderer;

        private void Start()
        {
            _gameResultSystem.IsUserWin += EndGame;
            _token = this.GetCancellationTokenOnDestroy();
        }

        private void EndGame(bool result)
        {
            OnDisposable();
            _direction = Vector2.zero;
        }

        public void OnSetUp(string enemyId, int indexPos)
        {
            IndexPos = indexPos;
            _enemyHandler = null;
            _enemyHandler = _mainGamePlaySystem.EnemiesController.GetAvailableEnemyHandler();

            _enemyHandler.OnSetUp(enemyId);
            _animationHander.OnSetUp(_enemyHandler);
            CanvasInGamePlayController.Instance.OnSetUpHpBar(_hpBarPos, _enemyHandler);

            SubcribeValue();
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

            _enemyHandler.DamageFeed += ShowDamageFeed;
        }

        private void ShowDamageFeed(int damge)
        {
            CanvasInGamePlayController.Instance.OnShowDamageFeed(_damageFeedPos, damge);
        }

        private void FixedUpdate()
        {
            _rb.velocity = _direction * _speed;
        }

        protected virtual void OnTriggerEnter2D(Collider2D collision)
        {
            OnTriggerToStop(collision);

            if (collision.tag == CollisionTagKey.BULLET_LEADER && gameObject.name == Leader.LeaderAction.Instance.NameObjectShoot)
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
            ReleaseObject();
        }

        private async void ReleaseObject()
        {
            try
            {
                await UniTask.Delay(2000, cancellationToken: _token);

                _tween = _spriteRenderer.DOFade(0.8f, _durationDoFade).OnComplete(() =>
                {
                    _spriteRenderer.DOFade(1f, _durationDoFade);
                }).SetLoops(3, LoopType.Yoyo);

                await _tween.AwaitForComplete(cancellationToken: _token);
                _mainGamePlaySystem.SpawnEnemiesHandler.RemoveEnemyToList(this);
                _spawnerManager.Release(gameObject);
            }
            catch (OperationCanceledException) { }
        }

        private void OnDisposable()
        {
            _disposableIsDeath?.Dispose();
            _disposableDirection?.Dispose();
            _disposableIsAttacking?.Dispose();
            _enemyHandler.DamageFeed -= ShowDamageFeed;
        }

        private void OnDestroy()
        {
            _tween?.Kill();
            _enemyHandler.DamageFeed -= ShowDamageFeed;
        }
    }
}