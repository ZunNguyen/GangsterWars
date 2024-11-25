using Game.Character.Enemy.Abstract;
using Game.Effect.MuzzleFlash;
using Sources.SpawnerSystem;
using Sources.Utils.Singleton;
using System;
using UnityEngine;

namespace Game.Character.Enemy.Shoot
{
    public class AnimationEnemyShootHandler : AnimationHandlerAbstract
    {
        private readonly Vector3 _rotateDefault = new Vector3(0, 0, 180f);

        private SpawnerManager _spawnerManager => Locator<SpawnerManager>.Instance;

        [SerializeField] private EnemyControllerAbstract _enemyController;
        [SerializeField] private WeaponAbstract _weaponPrefab;
        [SerializeField] private GameObject _muzzleFlash;
        [SerializeField] private Transform _weaponHolder;
        [SerializeField] private Transform _posSpawnWeapon;

        public override void OnAttack()
        {
            SpawnBullet(_posSpawnWeapon);
            SpawnMuzzleFlash(_posSpawnWeapon);
        }

        protected void SpawnBullet(Transform posSpawn)
        {
            var newWeapon = _spawnerManager.Get(_weaponPrefab);
            newWeapon.gameObject.SetActive(false);
            newWeapon.transform.position = posSpawn.position;
            newWeapon.transform.SetParent(_weaponHolder);
            newWeapon.OnSetUp(_enemyHandler, _enemyController.IndexPos);
        }

        protected void SpawnMuzzleFlash(Transform posSpawn)
        {
            if (_muzzleFlash == null) return;
            var muzzleFlash = _spawnerManager.Get(_muzzleFlash);
            muzzleFlash.transform.SetParent(_weaponHolder);
            muzzleFlash.transform.position = posSpawn.position;
            muzzleFlash.transform.rotation = Quaternion.Euler(_rotateDefault);
        }
    }
}