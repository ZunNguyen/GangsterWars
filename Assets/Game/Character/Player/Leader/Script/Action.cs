using Cysharp.Threading.Tasks;
using Game.Weapon.Bullet;
using Sources.GamePlaySystem.Leader;
using Sources.SpawnerSystem;
using Sources.Utils.Singleton;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.Character.Leader
{
    public class Action : MonoBehaviour
    {
        private SpawnerManager _spawnerManager => Locator<SpawnerManager>.Instance;
        private LeaderSystem _leaderSystem => Locator<LeaderSystem>.Instance;

        private bool _isCanShoot;

        [SerializeField] private Animation _animation;
        [SerializeField] private BulletMoveMent _bulletMoveMent;
        [SerializeField] private GameObject _muzzleFlash;
        [SerializeField] private Transform _posSpawnBullet;

        private void Awake()
        {
            _leaderSystem.IsCanShoot.Subscribe(value =>
            {
                _isCanShoot = value;
            }).AddTo(this);
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                if (!_isCanShoot) return;

                _animation.AnimationShoot();

                Vector3 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                clickPosition.z = 0;

                var bullet = _spawnerManager.Get<BulletMoveMent>(_bulletMoveMent);
                bullet.transform.position = _posSpawnBullet.position;
                bullet.MoveMent(clickPosition);

                var muzzleFlash = _spawnerManager.Get<GameObject>(_muzzleFlash);
                muzzleFlash.transform.position = _posSpawnBullet.position;

                _leaderSystem.UpdateBullet();
            }
        }
    }
}