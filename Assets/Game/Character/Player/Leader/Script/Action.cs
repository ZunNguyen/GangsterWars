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

        [SerializeField] private AnimationHandler _animation;
        [SerializeField] private LeaderWeapon _bullet;
        [SerializeField] private GameObject _muzzleFlash;
        [SerializeField] private Transform _posSpawnBullet;

        private void Awake()
        {
            _leaderSystem.GunHandler.IsShooting += AnimationShooting;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                _leaderSystem.GunHandler.Shooting();
            }
        }

        private void AnimationShooting()
        {
            _animation.AnimationShoot();

            Vector3 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            clickPosition.z = -1;

            var bullet = _spawnerManager.Get(_bullet);
            bullet.transform.position = _posSpawnBullet.position;
            bullet.MoveMent(clickPosition);

            var muzzleFlash = _spawnerManager.Get(_muzzleFlash);
            muzzleFlash.transform.position = _posSpawnBullet.position;
        }

        private void OnDestroy()
        {
            _leaderSystem.GunHandler.IsShooting -= AnimationShooting;
        }
    }
}