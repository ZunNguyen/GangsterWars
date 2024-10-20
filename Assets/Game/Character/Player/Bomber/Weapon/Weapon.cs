using DG.Tweening;
using Sources.DataBaseSystem;
using Sources.Utils;
using Sources.Utils.Singleton;
using UnityEngine;

namespace Game.Character.Bomber
{
    public class Weapon : MonoBehaviour
    {
        private const float _throwSpeed = 20f;
        private const float _height = 10f;
        private readonly Vector3 _offsetPosTarget = new Vector3(-3f,0,0);

        private DataBase _dataBase => Locator<DataBase>.Instance;
        private BomberConfig _bomberConfig => _dataBase.GetConfig<BomberConfig>();

        [SerializeField] private SpriteRenderer _sprite;

        public void GetIconWeapon(string bomId)
        {
            var bomInfo = _bomberConfig.GetWeaponInfo(bomId);
            _sprite.sprite = bomInfo.Icon;
        }

        public void ThrowBomb(Vector3 posTarget)
        {
            posTarget += _offsetPosTarget;
            var middlePoint = GetVector.GetHightPointBetweenTwoPoint(transform.position, posTarget, _height);

            Vector3[] path = new Vector3[]
            {
                transform.position,
                middlePoint,
                posTarget
            };

            var duration = TweenUtils.GetTimeDuration(transform.position, posTarget, _throwSpeed);
            transform.DOPath(path, duration, PathType.CatmullRom).SetEase(Ease.Linear).OnComplete(OnBombHit);
        }

        private void OnBombHit()
        {
            Debug.Log("Bomb hit target!");
        }
    }
}