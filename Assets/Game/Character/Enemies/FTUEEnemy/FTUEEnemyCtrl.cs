using Cysharp.Threading.Tasks;
using DG.Tweening;
using Game.CanvasInGamePlay;
using Game.Character.Sniper;
using Game.Screens.GamePlayScreen;
using Game.Weapon.Bullet;
using Sources.Extension;
using Sources.FTUE.System;
using Sources.GamePlaySystem.MainGamePlay;
using Sources.GamePlaySystem.MainGamePlay.Enemies;
using Sources.SpawnerSystem;
using Sources.Utils.Singleton;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace Game.Character.Enemy.FTUE
{
    public class FTUEEnemyCtrl : MonoBehaviour
    {
        private const float _speed = 0.7f;
        private const float _durationDoFade = 0.3f;
        private const float _timeToWalkTarget = 7f;

        private FTUESystem _ftueSystem => Locator<FTUESystem>.Instance;
        private SpawnerManager _spawnerManager => Locator<SpawnerManager>.Instance;

        private bool _firstDamage = false;
        private bool _isWalk = true;
        private bool _isCriticalDamage = false;
        private float _timeToWalk;
        private string _collisionKey;

        public ReactiveProperty<int> HPEnemy = new ReactiveProperty<int>(10);

        [SerializeField] private Rigidbody2D _rb;
        [SerializeField] private Animator _animator;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private FTUEDamageFeed _ftueDamageFeed;
        [SerializeField] private Transform _canvasHolder;
        [SerializeField] private Transform _posSpawnDamageFeed;

        private void Awake()
        {
            _animator.SetTrigger("Walk");
        }

        private void FixedUpdate()
        {
            if (HPEnemy.Value <= 0) return;

            if (!_isWalk)
            {
                _animator.SetTrigger("Idle");
                _rb.velocity = Vector3.zero;
                return;
            }

            _rb.velocity = Vector2.left * _speed;
            _timeToWalk += Time.deltaTime;
            if (_timeToWalk >= _timeToWalkTarget ) _isWalk = false;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag == CollisionTagKey.BULLET_LEADER)
            {
                FirstEnemyReciveDamage();

                var bullet = collision.GetComponent<LeaderWeapon>();
                bullet.ReleaseBullet();
                _collisionKey = bullet.CollisionKey;

                SubstractHp(bullet.Damage);
            }
        }

        private void FirstEnemyReciveDamage()
        {
            if (_firstDamage) return;

            _ftueSystem.TriggerWaitPoint(FTUEKey.WaitPoint_FirstEnemyReviceDamage);
            _firstDamage = true;
            _isCriticalDamage = true;
        }

        private void SubstractHp(int damage)
        {
            if (_isCriticalDamage && _collisionKey == CollisionTagKey.ENEMY_HEAD)
            {
                var damageFeed = _spawnerManager.Get(_ftueDamageFeed);
                damageFeed.transform.SetParent(_canvasHolder, false);
                damageFeed.transform.position = _posSpawnDamageFeed.position;
                damageFeed.ShowDamageFeed(damage * 2);

                HPEnemy.Value -= damage * 2;
            }
            else
            {
                HPEnemy.Value -= damage;
            }

            CheckDeath();
        }

        private void CheckDeath()
        {
            if (HPEnemy.Value <= 0) ReleaseObject();
        }

        private async void ReleaseObject()
        {
            _animator.SetTrigger("Death");
            await UniTask.Delay(2000);

            await _spriteRenderer.DOFade(0.8f, _durationDoFade).OnComplete(() =>
            {
                _spriteRenderer.DOFade(1f, _durationDoFade);
            }).SetLoops(3);

            _ftueSystem.TriggerWaitPoint(FTUEKey.WaitPoint_EnemyDeath);
            Destroy(gameObject);
        }
    }
}