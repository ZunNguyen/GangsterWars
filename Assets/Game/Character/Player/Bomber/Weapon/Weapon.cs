using Cysharp.Threading.Tasks;
using DG.Tweening;
using Sources.DataBaseSystem;
using Sources.GameData;
using Sources.SpawnerSystem;
using Sources.Utils;
using Sources.Utils.Singleton;
using UnityEngine;

namespace Game.Character.Bomber
{
    public class Weapon : MonoBehaviour
    {
        private const float _throwSpeed = 20f;
        private const float _height = 10f;
        private readonly Vector3 _offsetPosTarget = new Vector3(-1f,0,0);

        private SpawnerManager _spawnerManager => Locator<SpawnerManager>.Instance;
        private DataBase _dataBase => Locator<DataBase>.Instance;
        private BomberConfig _bomberConfig => _dataBase.GetConfig<BomberConfig>();

        private int _damage;
        public int Damage => _damage;

        [SerializeField] private SpriteRenderer _sprite;
        [SerializeField] private Animator _animator;
        [SerializeField] private Collider2D _collider;

        private void Awake()
        {
            SetEnabled(false);
        }

        private void SetEnabled(bool status)
        {
            _animator.enabled = status;
            _collider.enabled = status;
        }

        public void OnSetUp(WeaponData weaponData)
        {
            SetEnabled(false);

            var bomInfo = _bomberConfig.GetWeaponInfo(weaponData.WeaponId);

            _sprite.sprite = bomInfo.Icon;
            var levelInfo = bomInfo.GetLevelUpgradeInfo(weaponData.LevelUpgradeId);
            _damage = levelInfo.Damage;
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

        private async void OnBombHit()
        {
            await UniTask.Delay(500);
            SetEnabled(true);
        }

        public void OnCompletAnimation()
        {
            _spawnerManager.Release(this);
        }
    }
}