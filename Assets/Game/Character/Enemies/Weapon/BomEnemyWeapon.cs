using DG.Tweening;
using Sources.Utils;
using UnityEngine;

namespace Game.Character.Enemy.Weapon
{
    public class BomEnemyWeapon : Abstract.WeaponAbstract
    {
        private const float _throwSpeed = 20f;
        private const float _height = 2f;
        private const float _randomPosXMin = -1f;
        private const float _randomPosXMax = 1f;

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
            transform.DOPath(path, duration, PathType.CatmullRom).SetEase(Ease.Linear).OnComplete(OnBombHit);
        }
    }
}