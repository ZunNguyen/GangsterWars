using Sources.DataBaseSystem;
using Sources.GameData;
using Sources.Utils.Singleton;
using Sources.Utils.String;
using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

namespace Sources.GamePlaySystem.MainMenuGame
{
    public class WeaponViewModel
    {
        public ReactiveProperty<WeaponState> State = new();
        public ReactiveProperty<int> LevelUpgradeFee = new();
        public ReactiveProperty<int> ReloadFee = new();
        public List<string> LevelUpgradeIdsPassed = new();
        public int UnlockFee;
    }

    public class StoreWeaponHandler
    {
        private GameData.GameData _gameData => Locator<GameData.GameData>.Instance;
        private StoreProfile _storeProfile => _gameData.GetProfileData<StoreProfile>();

        private DataBase _dataBase => Locator<DataBase>.Instance;
        private StoreConfig _storeConfig => _dataBase.GetConfig<StoreConfig>();

        private List<WeaponData> _weaponDatas;
        private List<WeaponInfo> _weaponConfigs = new();
        private int _weaponIndexMaxCurrent;

        public Dictionary<string, WeaponViewModel> WeaponWiewModels { get; private set; } = new();
        public bool HadStore { get; private set; } = false;
        public Action<string> NewWeapon;

        public void OnSetUp(List<WeaponData> weaponDatas, List<WeaponInfo> weaponConfigs)
        {
            _weaponDatas = weaponDatas;
            _weaponConfigs = weaponConfigs;

            var weaponLargest = _weaponDatas[_weaponDatas.Count - 1];
            _weaponIndexMaxCurrent = GetIndexInList(weaponLargest.WeaponId, _weaponConfigs);
            HadStore = weaponDatas != null;

            SetWeaponViewModels();
        }

        private void SetWeaponViewModels()
        {
            foreach (var weaponConfig in _weaponConfigs)
            {
                var newWeaponViewModel = new WeaponViewModel();
                WeaponWiewModels.Add(weaponConfig.Id, newWeaponViewModel);
                var weaponData = new WeaponData
                {
                    WeaponId = weaponConfig.Id,
                    LevelUpgradeId = weaponConfig.LevelUpgrades[0].Id,
                };
                UpdateWeaponViewModel(weaponData);
            }

            for (int i = 0; i < _weaponDatas.Count; i++)
            {
                UpdateWeaponViewModel(_weaponDatas[i]);
            }

            foreach (var weaponDebug in WeaponWiewModels)
            {
                Debug.Log($"{weaponDebug.Value.State} / {weaponDebug.Value.UnlockFee} / {weaponDebug.Value.LevelUpgradeFee}" +
                    $" / {weaponDebug.Value.ReloadFee} / {weaponDebug.Value.LevelUpgradeIdsPassed.Count}");
            }
        }

        private void UpdateWeaponViewModel(WeaponData weaponData)
        {
            var weaponViewModel = WeaponWiewModels[weaponData.WeaponId];

            weaponViewModel.State.Value = GetWeaponState(weaponData.WeaponId);
        
            var weaponInfo = _storeConfig.GetWeaponInfo(weaponData.WeaponId);
            weaponViewModel.UnlockFee = weaponInfo.UnlockFee;

            var levelUpgradeInfo = weaponInfo.GetLevelUpgradeInfo(weaponData.LevelUpgradeId);
            weaponViewModel.ReloadFee.Value = levelUpgradeInfo.ReloadFee;

            var indexLevelUpgradeCurrent = weaponInfo.GetIndexLevelUpgrade(weaponData.LevelUpgradeId);
            if (indexLevelUpgradeCurrent != weaponInfo.LevelUpgrades.Count - 1)
            {
                var levelUpgradeNextInfo = weaponInfo.LevelUpgrades[indexLevelUpgradeCurrent + 1];
                weaponViewModel.LevelUpgradeFee.Value = levelUpgradeNextInfo.LevelUpFee;
            }

            weaponViewModel.LevelUpgradeIdsPassed.Clear();
            for (int i = 0; i <= indexLevelUpgradeCurrent; i++)
            {
                weaponViewModel.LevelUpgradeIdsPassed.Add(weaponInfo.LevelUpgrades[i].Id);
            }
        }

        public WeaponState GetWeaponState(string weaponId)
        {
            var weaponIndex = GetIndexInList(weaponId, _weaponConfigs);

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
            var weapon = _weaponConfigs.FirstOrDefault(weapon => weapon.Id == weaponId);
            var weaponIndex = _weaponConfigs.IndexOf(weapon);

            var levelInfoCurrent = weapon.LevelUpgrades.FirstOrDefault(level => level.Id == levelCurrent);
            var levelInfoIndexCurrent = weapon.LevelUpgrades.IndexOf(levelInfoCurrent);
            if (levelInfoIndexCurrent == weapon.LevelUpgrades.Count - 1) return;
            levelCurrent = weapon.LevelUpgrades[levelInfoIndexCurrent + 1].Id;

            var weaponData = _weaponDatas.FirstOrDefault(weapon => weapon.WeaponId == weaponId);
            var weaponDataIndex = _weaponDatas.IndexOf(weaponData);
            _weaponDatas[weaponDataIndex].LevelUpgradeId = levelCurrent;

            _storeProfile.Save();
        }

        private int GetIndexInList(string id, List<WeaponInfo> weaponsConfig)
        {
            var weapon = weaponsConfig.FirstOrDefault(weapon => weapon.Id == id);
            return weaponsConfig.IndexOf(weapon);
        }

        public bool IsHandlerSystem(string weaponId)
        {
            var baseWeapon = StringUtils.GetBaseName(weaponId);
            var baseWeaponSystem = StringUtils.GetBaseName(_weaponDatas[0].WeaponId);

            return baseWeaponSystem == baseWeapon;
        }
    }
}