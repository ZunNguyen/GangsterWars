using Sources.DataBaseSystem;
using Sources.Extension;
using Sources.GameData;
using Sources.GamePlaySystem.CoinController;
using Sources.Utils.Singleton;
using Sources.Utils.String;
using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEditor;
using UnityEngine;

namespace Sources.GamePlaySystem.MainMenuGame
{
    public class WeaponViewModel
    {
        public ReactiveProperty<WeaponState> State = new();
        public ReactiveProperty<int> LevelUpgradeFee = new(0);
        public List<string> LevelUpgradeIdsPassed = new();
        public int UnlockFee;
        public int ReloadFee;
    }

    public class StoreWeaponHandler
    {
        private const int _levelUpgardeFeeDefault = 0;

        private GameData.GameData _gameData => Locator<GameData.GameData>.Instance;
        private UserProfile _userProfile => _gameData.GetProfileData<UserProfile>();

        private DataBase _dataBase => Locator<DataBase>.Instance;
        private StoreConfig _storeConfig => _dataBase.GetConfig<StoreConfig>();

        private CoinControllerSystem _coinControllerSystem => Locator<CoinControllerSystem>.Instance;

        private List<WeaponData> _weaponDatas;
        private List<WeaponInfo> _weaponConfigs = new();
        private int _weaponIndexMaxCurrent;

        public Dictionary<string, WeaponViewModel> WeaponWiewModels { get; private set; } = new();
        public bool HadStore { get; private set; } = false;

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
        }

        private void UpdateWeaponViewModel(WeaponData weaponData)
        {
            var weaponViewModel = WeaponWiewModels[weaponData.WeaponId];

            weaponViewModel.State.Value = GetWeaponState(weaponData.WeaponId);
        
            var weaponInfo = _storeConfig.GetWeaponInfo(weaponData.WeaponId);
            weaponViewModel.UnlockFee = weaponInfo.UnlockFee;

            var levelUpgradeInfo = weaponInfo.GetLevelUpgradeInfo(weaponData.LevelUpgradeId);
            var bulletRemain = _userProfile.GetWeaponData(weaponData.WeaponId).Quatity;
            var reloadFee = levelUpgradeInfo.ReloadFee;
            weaponViewModel.ReloadFee = levelUpgradeInfo.ReloadFee;





            var indexLevelUpgradeCurrent = weaponInfo.GetIndexLevelUpgrade(weaponData.LevelUpgradeId);
            if (indexLevelUpgradeCurrent != weaponInfo.LevelUpgrades.Count - 1)
            {
                var levelUpgradeNextInfo = weaponInfo.LevelUpgrades[indexLevelUpgradeCurrent + 1];
                weaponViewModel.LevelUpgradeFee.Value = levelUpgradeNextInfo.LevelUpFee;
            }
            else weaponViewModel.LevelUpgradeFee.Value = _levelUpgardeFeeDefault;

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

        public void UnlockNewWeapon(string weaponId)
        {
            var weaponViewModel = WeaponWiewModels[weaponId];
            var fee = weaponViewModel.UnlockFee;
            bool result = _coinControllerSystem.PurchaseItem(fee);

            if (result)
            {
                weaponViewModel.State.Value = WeaponState.AlreadyHave;
                var newWeaponData = new WeaponData
                {
                    WeaponId = weaponId,
                    LevelUpgradeId = LevelUpgradeKey.LevelUpgrade_Default,
                };
                _weaponDatas.Add(newWeaponData);
                _userProfile.Save();

                var weaponLargest = _weaponDatas[_weaponDatas.Count - 1];
                _weaponIndexMaxCurrent = GetIndexInList(weaponLargest.WeaponId, _weaponConfigs);

                foreach (var weaponConfig in _weaponConfigs)
                {
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

                foreach (var weaponModel in WeaponWiewModels)
                {
                    Debug.Log($"{weaponModel.Value.State}");
                }

                Debug.Log($"Unlock {weaponId} successfully");
            }
            else Debug.Log("Not enough money!");
        }

        public void UpgradeNewLevelWeapon(string weaponId)
        {
            var weaponModel = WeaponWiewModels[weaponId];
            if (weaponModel.LevelUpgradeFee.Value == _levelUpgardeFeeDefault)
            {
                Debug.Log($"Level Upgrade {weaponModel.LevelUpgradeFee.Value} is max");
                return;
            }

            var result = _coinControllerSystem.PurchaseItem(weaponModel.LevelUpgradeFee.Value);
            if (result)
            {
                var weaponConfig = _storeConfig.GetWeaponInfo(weaponId);
                var levelInfoIndexCurrent = weaponConfig.GetIndexLevelUpgrade(weaponModel.LevelUpgradeIdsPassed[weaponModel.LevelUpgradeIdsPassed.Count - 1]);
                var levelNextId = weaponConfig.LevelUpgrades[levelInfoIndexCurrent + 1].Id;

                weaponModel.LevelUpgradeIdsPassed.Add(levelNextId);
                var weaponData = _userProfile.GetWeaponData(weaponId);
                weaponData.LevelUpgradeId = levelNextId;

                UpdateWeaponViewModel(weaponData);
                _userProfile.Save();
            }
            else Debug.Log("Not enough money!");
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

        private WeaponData GetWeaponData(string weaponId)
        {
            return _weaponDatas.FirstOrDefault(weapon => weapon.WeaponId == weaponId);
        }
    }
}