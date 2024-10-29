using Sources.DataBaseSystem;
using Sources.GameData;
using Sources.Utils;
using Sources.Utils.Singleton;
using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;

namespace Sources.GamePlaySystem.MainMenuGame
{
    public class StoreWeaponHandler
    {
        private GameData.GameData _gameData => Locator<GameData.GameData>.Instance;
        private StoreProfile _storeProfile => _gameData.GetProfileData<StoreProfile>();

        private List<WeaponData> _weaponsData;
        private List<WeaponInfo> _weaponsConfig = new();
        private int _weaponIndexMaxCurrent;

        public bool HadStore { get; private set; } = false;
        public Action<string> NewWeapon;

        public void OnSetUp(List<WeaponData> weaponsData, List<WeaponInfo> weaponsConfig)
        {
            _weaponsData = weaponsData;
            _weaponsConfig = weaponsConfig;

            var weaponLargest = _weaponsData[_weaponsData.Count - 1];
            _weaponIndexMaxCurrent = GetIndexInList(weaponLargest.WeaponId, _weaponsConfig);
            HadStore = weaponsData != null;
        }

        public WeaponState GetWeaponState(string weaponId)
        {
            var weaponIndex = GetIndexInList(weaponId, _weaponsConfig);

            if (weaponIndex <= _weaponIndexMaxCurrent) return WeaponState.AlreadyHave;
            if (weaponIndex == (_weaponIndexMaxCurrent + 1)) return WeaponState.CanUnlock;
            else return WeaponState.CanNotUnlock;
        }

        public void UpgradeNewWeapon(string weaponId)
        {
            NewWeapon?.Invoke(weaponId);
        }

        public void UpgradeNewLevelWeapon(string weaponId, string levelCurrent)
        {
            var weapon = _weaponsConfig.FirstOrDefault(weapon => weapon.Id == weaponId);
            var weaponIndex = _weaponsConfig.IndexOf(weapon);

            var levelInfoCurrent = weapon.LevelUpgrades.FirstOrDefault(level => level.Id == levelCurrent);
            var levelInfoIndexCurrent = weapon.LevelUpgrades.IndexOf(levelInfoCurrent);
            if (levelInfoIndexCurrent == weapon.LevelUpgrades.Count - 1) return;
            levelCurrent = weapon.LevelUpgrades[levelInfoIndexCurrent + 1].Id;

            var weaponData = _weaponsData.FirstOrDefault(weapon => weapon.WeaponId == weaponId);
            var weaponDataIndex = _weaponsData.IndexOf(weaponData);
            _weaponsData[weaponDataIndex].LevelUpgradeId = levelCurrent;

            _storeProfile.Save();
        }

        private int GetIndexInList(string id, List<WeaponInfo> weaponsConfig)
        {
            var weapon = weaponsConfig.FirstOrDefault(weapon => weapon.Id == id);
            return weaponsConfig.IndexOf(weapon);
        }
    }
}