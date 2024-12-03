using Sources.GamePlaySystem.Character;
using Sources.SpawnerSystem;
using Sources.Utils.Singleton;
using UnityEngine;

namespace Game.Character.Player.Abstract
{
    public abstract class ActionHandlerAbstract : MonoBehaviour
    {
        private SpawnerManager _spawnerManager => Locator<SpawnerManager>.Instance;

        protected WeaponHandler _weaponHandler;

        private WeaponAbstract _weapon;

        [SerializeField] private WeaponAbstract _weaponPrefab;
        [SerializeField] private Transform _weaponPos;
        [SerializeField] private Transform _weaponHolder;

        public void OnSetUp()
        {
            GetHandlerSystem();
        }

        protected abstract void GetHandlerSystem();

        public void Attack()
        {
            var damageWeapon = _weaponHandler.GetDamageWeapon();

            _weapon = _spawnerManager.Get(_weaponPrefab);
            _weapon.OnSetUp(_weaponHandler.WeaponIdCurrent, damageWeapon);
            _weapon.transform.SetParent(_weaponHolder);
            _weapon.transform.position = _weaponPos.position;
            _weapon.gameObject.SetActive(false);
        }

        public void Throwing()
        {
            _weapon.gameObject.SetActive(true);
            _weapon.Moving();
            _weaponHandler.EndActionThrow();
        }
    }
}