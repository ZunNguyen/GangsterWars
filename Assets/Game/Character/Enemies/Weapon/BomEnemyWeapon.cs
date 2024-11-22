using DG.Tweening;
using Sources.Utils;
using UnityEngine;

namespace Game.Character.Enemy.Weapon
{
    public class BomEnemyWeapon : Abstract.WeaponAbstract
    {
        private const float _throwSpeed = 20f;
        private const float _height = 10f;
        private readonly Vector3 _offsetPosTarget = new Vector3(-1f, 0, 0);

        protected override void Moving()
        {
            _targetPos += _offsetPosTarget;
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