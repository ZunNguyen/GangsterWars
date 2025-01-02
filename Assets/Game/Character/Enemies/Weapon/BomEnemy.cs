using Cysharp.Threading.Tasks;
using DG.Tweening;
using Game.Character.Controller;
using Sources.Audio;
using Sources.Extension;
using Sources.GamePlaySystem.MainGamePlay;
using Sources.GamePlaySystem.MainGamePlay.Enemies;
using Sources.Utils;
using Sources.Utils.Singleton;
using UnityEngine;

namespace Game.Character.Enemy.Weapon
{
    public class BomEnemy : Abstract.WeaponAbstract
    {
        private readonly Vector3 _defaultScale = new Vector3(0.7f, 0.7f, 0.7f);
        private const float _throwSpeed = 20f;
        private const float _height = 2f;
        private const float _randomPosXMin = -1f;
        private const float _randomPosXMax = 1f;
        private const float _defaultPosZ = -2f;

        private AudioManager _audioManager => Locator<AudioManager>.Instance;

        private Vector3 _targetPos;

        [SerializeField] private Animator _animator;
        [SerializeField] private SpriteRenderer _sprite;
        [SerializeField] private Sprite _spriteOrigin;

        protected override void Moving()
        {
            var offsetPosx = GetRandom.GetRandomFloat(_randomPosXMin, _randomPosXMax);
            _targetPos.x += offsetPosx;
            var middlePoint = GetVector.GetHightPointBetweenTwoPoint(transform.position, _targetPos, _height);

            Vector3[] path = new Vector3[]
            {
                transform.position,
                middlePoint,
                _targetPos
            };

            var duration = TweenUtils.GetTimeDuration(transform.position, _targetPos, _throwSpeed);
            transform.DOPath(path, duration, PathType.CatmullRom).SetEase(Ease.Linear).OnComplete(DamageUser);
        }

        private void SetEnabled(bool status)
        {
            _animator.enabled = status;
        }

        public override void OnSetUp(EnemyHandler enemyHandler, int indexPos)
        {
            SetEnabled(false);
            SetPosZ();
            transform.localScale = _defaultScale;
            _sprite.sprite = _spriteOrigin;
            GetTargetPos(indexPos);
            base.OnSetUp(enemyHandler, indexPos);
        }

        private void SetPosZ()
        {
            Vector3 newPosition = transform.position;
            newPosition.z = _defaultPosZ;
            transform.position = newPosition;
        }

        private void GetTargetPos(int indexPos)
        {
            _targetPos = _mainGamePlaySystem.EnemiesController.ShieldPlayerPos[indexPos].position;
        }

        protected override async void DamageUser()
        {
            await UniTask.Delay(500);
            SetEnabled(true);
            _audioManager.Play(AudioKey.SFX_BOOM_01);
            base.DamageUser();
        }
    }
}