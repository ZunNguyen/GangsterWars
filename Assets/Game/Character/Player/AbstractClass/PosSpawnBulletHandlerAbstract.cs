using Cysharp.Threading.Tasks;
using Game.Character.BulletEffect;
using Game.Character.Leader;
using Game.Weapon.Bullet;
using Sources.GamePlaySystem.Leader;
using Sources.SpawnerSystem;
using Sources.Utils.Singleton;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace Game.Character.Player.Abstract
{
    public abstract class PosSpawnBulletHandlerAbstract : MonoBehaviour
    {
        private readonly Vector3 _rotateDefault = Vector3.zero;

        private SpawnerManager _spawnerManager => Locator<SpawnerManager>.Instance;
        private LeaderSystem _leaderSystem => Locator<LeaderSystem>.Instance;

        protected bool _isCanShoot = false;
        protected string _gunId;

        [Header("Muzzle Flash")]
        [SerializeField] private GameObject _muzzleFlash;
        [SerializeField] private Transform _muzzleFlashHolder;

        [Header("Bullet")]
        [SerializeField] private LeaderWeapon _weaponPrefab;
        [SerializeField] private Transform _weaponHolders;
        [SerializeField] protected List<Transform> _posSpawns;

        [Header("Hit Bullet Effect")]
        [SerializeField] private HitBulletEffect _hitBulletEffectPrefab;

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
            muzzleFlash.transform.rotation = Quaternion.Euler(_rotateDefault);
        }

        protected void SpawnBullet(Transform posSpawn, Vector3 posClick)
        {
            var bullet = _spawnerManager.Get(_weaponPrefab);
            bullet.transform.SetParent(_weaponHolders);
            bullet.transform.position = posSpawn.position;
            bullet.OnSetUp(_hitBulletEffectPrefab, posClick);
        }

        private void OnDestroy()
        {
            _leaderSystem.GunHandler.IsShooting -= Shooting;
        }
    }
}