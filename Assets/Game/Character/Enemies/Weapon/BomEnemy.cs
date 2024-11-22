using Cysharp.Threading.Tasks;
using DG.Tweening;
using Sources.GamePlaySystem.MainGamePlay;
using Sources.GamePlaySystem.MainGamePlay.Enemies;
using Sources.Utils;
using UnityEngine;

namespace Game.Character.Enemy.Weapon
{
    public class BomEnemy : Abstract.WeaponAbstract
    {
        private const float _throwSpeed = 20f;
        private const float _height = 2f;
        private const float _randomPosXMin = -1f;
        private const float _randomPosXMax = 1f;

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
            _sprite.sprite = _spriteOrigin;
            GetTargetPos(indexPos);
            base.OnSetUp(enemyHandler, indexPos);
        }

        private void GetTargetPos(int indexPos)
        {
            _targetPos = _mainGamePlaySystem.EnemiesController.ShieldPlayerPos[indexPos].position;
        }

        protected override async void DamageUser()
        {
            await UniTask.Delay(500);
            SetEnabled(true);
            base.DamageUser();
        }
    }
}