using Cysharp.Threading.Tasks;
using Sources.GamePlaySystem.Leader;
using Sources.Utils.Singleton;
using UnityEngine;
using UniRx;
using Game.Weapon.Bullet;
using System.Collections.Generic;
using Sources.SpawnerSystem;
using static UnityEditor.PlayerSettings;

namespace Game.Character.Abstract
{
    public abstract class PosSpawnBulletHandlerAbstract : MonoBehaviour
    {
        public Vector3 _offsetTargetPosMouseClick = new Vector3(1, 1, -1);

        protected SpawnerManager _spawnerManager => Locator<SpawnerManager>.Instance;
        private LeaderSystem _leaderSystem => Locator<LeaderSystem>.Instance;

        protected bool _isCanShoot = false;
        protected string _gunId;

        [Header("Muzzle Flash")]
        [SerializeField] protected GameObject _muzzleFlash;
        [SerializeField] protected Transform _muzzleFlashHolder;

        [Header("Bullet")]
        [SerializeField] protected LeaderWeapon _weaponPrefab;
        [SerializeField] protected Transform _weaponHolders;
        [SerializeField] protected List<Transform> _posSpawns;

        private void Awake()
        {
            OnSetUp();
            _leaderSystem.GunHandler.IsShooting += Shooting;

            _leaderSystem.GunHandler.GunModelCurrent.Subscribe(value =>
            {
                if (value.GunId != _gunId) _isCanShoot = false;
                else _isCanShoot = true;
            }).AddTo(this);
        }

        protected abstract void OnSetUp();

        protected abstract void Shooting();

        protected void SpawnMuzzleFlash()
        {
            var muzzleFlash = _spawnerManager.Get(_muzzleFlash);
            muzzleFlash.transform.SetParent(_muzzleFlashHolder);
            muzzleFlash.transform.position = transform.position;
        }

        protected void SpawnBullet(Transform posSpawn, Vector3 posClick)
        {
            var bullet = _spawnerManager.Get(_weaponPrefab);
            bullet.transform.SetParent(_weaponHolders);
            bullet.transform.position = posSpawn.position;
            bullet.MoveMent(posClick);
        }

        private void OnDestroy()
        {
            _leaderSystem.GunHandler.IsShooting -= Shooting;
        }
    }
}