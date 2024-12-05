using Sources.DataBaseSystem;
using Sources.GamePlaySystem.MainMenuGame;
using Sources.Utils.Singleton;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace Game.Screens.MainMenuScreen
{
    public class StoreWeaponHandler : MonoBehaviour
    {
        private StoreSystem _storeSystem => Locator<StoreSystem>.Instance;

        private TabState _tabState;

        [SerializeField] private TabHandler _tabHandler;

        [Header("Weapon View")]
        [SerializeField] private WeaponView _weaponViewPrefab;
        [SerializeField] private Transform _holderWeaponView;
        [SerializeField] private GameObject _store;

        public ReactiveProperty<bool> IsChosed;

        private void Awake()
        {
            _store.SetActive(false);
        }

        public void OnSetUp(IEnumerable<WeaponInfoBase> weaponsConfig)
        {
            foreach (var weapon in weaponsConfig)
            {
                var newWeaponPrefab = Instantiate(_weaponViewPrefab, _holderWeaponView);
                newWeaponPrefab.OnSetUp(weapon, IsChosed);
            }
        }

        public void SetState(TabState state)
        {
            _tabState = state;
            _storeSystem.TabCurrent.Subscribe(ListenTabStateChange).AddTo(this);
        }

        private void ListenTabStateChange(TabState state)
        {
            _store.SetActive(state == _tabState);
            IsChosed.Value = state == _tabState;
        }
    }
}