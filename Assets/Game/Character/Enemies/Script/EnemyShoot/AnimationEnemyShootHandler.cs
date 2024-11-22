using Game.Character.Enemy.Abstract;
using Sources.SpawnerSystem;
using Sources.Utils.Singleton;
using System;
using UnityEngine;

namespace Game.Character.Enemy.Shoot
{
    public class AnimationEnemyShootHandler : AnimationHandlerAbstract
    {
        private SpawnerManager _spawnerManager => Locator<SpawnerManager>.Instance;

        [SerializeField] private WeaponAbstract _weaponPrefab;
        [SerializeField] private Transform _weaponHolder;
        [SerializeField] private Transform _posSpawnWeapon;

        public void OnShootWeapon()
        {
            var newWeapon = _spawnerManager.Get(_weaponPrefab);
            newWeapon.transform.SetParent(_weaponHolder);
            newWeapon.transform.position = _posSpawnWeapon.position;
            newWeapon.OnSetUp(_enemyHandler);
        }

        public override void OnAttackEnd()
        {
            _enemyHandler.SetIdle();
        }
    }
}