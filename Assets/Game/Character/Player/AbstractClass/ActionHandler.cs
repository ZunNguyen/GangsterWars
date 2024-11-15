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

        protected abstract void GetHandlerSystem();

        private async void Awake()
        {
            GetHandlerSystem();

            await UniTask.DelayFrame(1);

            _weaponHandler.WeaponCurrent.Subscribe(value =>
            {
                if (value == null) return;
                _weapon = _spawnerManager.Get(_weaponPrefab);
                _weapon.OnSetUp(value);
                _weapon.transform.position = _weaponPos.position;
                _weapon.gameObject.SetActive(false);

            }).AddTo(this);
        }

        public void Throwing()
        {
            _weapon.gameObject.SetActive(true);
            _weapon.ThrowBomb();
            _weaponHandler.EndActionThrow();
        }
    }
}