using DG.Tweening;
using Game.Character.BulletEffect;
using Sources.Audio;
using Sources.Extension;
using Sources.GamePlaySystem.MainGamePlay.Enemies;
using Sources.Utils.Singleton;
using UnityEngine;

namespace Game.Character.Enemy.Weapon
{
    public class BulletEnemy : Abstract.WeaponAbstract
    {
        private readonly Vector3 _scaleDefault = new Vector3(0.4f, 0.3f, 1f);
        private const float _offsetTargetX = 10f;
        private const float _duration = 0.6f;

        private AudioManager _audioManager => Locator<AudioManager>.Instance;

        private Vector2 _originalPos;
        private Tween _tween;

        [SerializeField] private HitBulletEffect _hitBulletEffect;
        [SerializeField] private Rigidbody2D _rb;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag == CollisionTagKey.SHIELD_USER)
            {
                DamageUser();
                OnRelease();
                SpawnHitBulletEffect();
            }
        }

        private void OnEnable()
        {
            _originalPos = transform.position;
        }

        public override void OnSetUp(EnemyHandler enemyHandler, int indexPos)
        {
            transform.localScale = _scaleDefault;
            base.OnSetUp(enemyHandler, indexPos);
        }

        protected override void Moving()
        {
            _audioManager.Play(AudioKey.SFX_SHOOT_PISTOL);
            var targetPosX = transform.position.x - _offsetTargetX;

            _tween = transform.DOMoveX(targetPosX, _duration).SetEase(Ease.InOutSine).OnComplete(() =>
            {
                OnRelease();
            });
        }

        private void SpawnHitBulletEffect()
        {
            var hitBulletEffect = _spawnerManager.Get(_hitBulletEffect);
            hitBulletEffect.transform.position = transform.position;
            hitBulletEffect.OnSetUp(_originalPos, transform.position);
        }

        public override void OnRelease()
        {
            _tween?.Kill();
            base.OnRelease();
        }
    }
}