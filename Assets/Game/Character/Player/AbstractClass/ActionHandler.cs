using Cysharp.Threading.Tasks;
using Sources.GamePlaySystem.Character;
using Sources.GamePlaySystem.MainGamePlay;
using Sources.SpawnerSystem;
using Sources.Utils.Singleton;
using UniRx;
using UnityEngine;

namespace Game.Character.Abstract
{
    public abstract class ActionHandler : MonoBehaviour
    {
        private SpawnerManager _spawnerManager => Locator<SpawnerManager>.Instance;

        protected WeaponHandler _weaponHandler;

        private Bomber.Weapon _weapon;

        [SerializeField] private Bomber.Weapon _weaponPrefab;
        [SerializeField] private Transform _weaponPos;

        private void Awake()
        {
            GetHandlerSystem();
        }

        protected abstract void GetHandlerSystem();

        public void Attack()
        {
            _weapon = _spawnerManager.Get(_weaponPrefab);
            _weapon.OnSetUp(_weaponHandler.WeaponCurrent.Value.Id);
            _weapon.transform.position = _weaponPos.position;
            _weapon.gameObject.SetActive(false);
        }

        public void Throwing()
        {
            _weapon.gameObject.SetActive(true);
            _weapon.ThrowBomb();
            _weaponHandler.EndActionThrow();
        }
    }
}