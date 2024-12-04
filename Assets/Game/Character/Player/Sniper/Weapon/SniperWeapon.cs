using DG.Tweening;
using Game.Character.Player.Abstract;
using Sources.Audio;
using Sources.Extension;
using Sources.Utils.Singleton;
using UnityEngine;

namespace Game.Character.Sniper
{
    public class SniperWeapon : WeaponAbstract
    {
        private const float _duration = 0.4f;
        private const float _factorOffsetPos = 15f;
        private const float _offsetEnemyTargetPosY = 2f;

        private AudioManager _audioManager => Locator<AudioManager>.Instance;

        private Vector3 _originPos;

        public string CollisionKey { get; private set; }

        [SerializeField] private TrailRenderer _trailRenderer;

        public override void Moving()
        {
            _audioManager.Play(AudioKey.SFX_SHOOT_SNIPER);

            var enemyTarget = _mainGamePlaySystem.SpawnEnemiesHandler.Enemies[0];
            if (enemyTarget == null) return;

            var enemyPos =  enemyTarget.transform.position;
            enemyPos.y += _offsetEnemyTargetPosY;

            Vector2 direction = (enemyPos - transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

            var targetPos = (Vector2)enemyPos + direction * _factorOffsetPos;

            _trailRenderer.Clear();
            transform.DOMove(targetPos, _duration).SetEase(Ease.OutExpo)
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