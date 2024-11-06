using Sources.DataBaseSystem;
using Sources.DataBaseSystem.Leader;
using Sources.GameData;
using Sources.Utils.Singleton;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Screens.MainMenuScreen
{
    public class StoreWeaponHandler : MonoBehaviour
    {
        private TabState _tabState;

        [SerializeField] private TabHandler _tabHandler;

        [Header("Weapon View")]
        [SerializeField] private WeaponView _weaponViewPrefab;
        [SerializeField] private Transform _holderWeaponView;

        public void OnSetUp(IEnumerable<WeaponInfoBase> weaponsConfig)
        {
            foreach (var weapon in weaponsConfig)
            {
                var newWeaponPrefab = Instantiate(_weaponViewPrefab, _holderWeaponView);
                newWeaponPrefab.OnSetUp(weapon);
            }
        }

        public void SetState(TabState state)
        {
            _tabState = state;
            _tabHandler.TabStateChange += ListenTabStateChange;
        }

        private void ListenTabStateChange(TabState state)
        {
            if (state != _tabState) gameObject.SetActive(false);
            else gameObject.SetActive(true);
        }
    }
}