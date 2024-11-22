using Cysharp.Threading.Tasks;
using DG.Tweening;
using Game.Character.Player.Abstract;
using Sources.DataBaseSystem;
using Sources.Utils;
using Sources.Utils.Singleton;
using UnityEngine;

namespace Game.Character.Bomber
{
    public class BomberWeapon : WeaponAbstract
    {
        private const float _throwSpeed = 20f;
        private const float _height = -2f;
        private readonly Vector3 _offsetPosTarget = new Vector3(-3f,0,0);

        private DataBase _dataBase => Locator<DataBase>.Instance;
        private BomberConfig _bomberConfig => _dataBase.GetConfig<BomberConfig>();

        [SerializeField] private SpriteRenderer _sprite;
        [SerializeField] private Animator _animator;
        [SerializeField] private Collider2D _collider;

        private void SetEnabled(bool status)
        {
            _animator.enabled = status;
            _collider.enabled = status;
        }

        public override void OnSetUp(string weaponId, int damage)
        {
            SetEnabled(false);
            var bomInfo = _bomberConfig.GetWeaponInfo(weaponId) as BomberWeaponInfo;
            _sprite.sprite = bomInfo.Icon;
            
            base.OnSetUp(weaponId, damage);
        }

        public override void Moving()
        {
            var enemyTarget = _mainGamePlaySystem.SpawnEnemiesHandler.Enemies[0];
            if (enemyTarget == null) return;

            var enemyPos = enemyTarget.transform.position;
            enemyPos += _offsetPosTarget;
            var middlePoint = GetVector.GetHightPointBetweenTwoPoint(transform.position, enemyPos, _height);

            Vector3[] path = new Vector3[]
            {
                transform.position,
                middlePoint,
                enemyPos
            };

            var duration = TweenUtils.GetTimeDuration(transform.position, enemyPos, _throwSpeed);
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