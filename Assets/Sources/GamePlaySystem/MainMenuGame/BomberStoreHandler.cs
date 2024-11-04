using Sources.DataBaseSystem.Leader;
using Sources.DataBaseSystem;
using Sources.Extension;
using Sources.GameData;
using Sources.GamePlaySystem.CoinController;
using Sources.GamePlaySystem.MainMenuGame.Store;
using Sources.Utils.Singleton;
using System.Collections.Generic;
using UnityEngine;
using Sources.Utils.String;

namespace Sources.GamePlaySystem.MainMenuGame
{
    public class BomberStoreHandler : StoreHandlerBase
    {
        private const int _levelUpgardeFeeDefault = 0;

        private GameData.GameData _gameData => Locator<GameData.GameData>.Instance;
        private UserProfile _userProfile => _gameData.GetProfileData<UserProfile>();

        private DataBase _dataBase => Locator<DataBase>.Instance;
        private BomberConfig _bomberConfig => _dataBase.GetConfig<BomberConfig>();

        private CoinControllerSystem _coinControllerSystem => Locator<CoinControllerSystem>.Instance;

        private List<WeaponData> _weaponDatas;
        private List<DataBaseSystem.Leader.WeaponInfo> _weaponConfigs;
        private int _weaponIndexMaxCurrent;

        public Dictionary<string, WeaponViewModel> WeaponWiewModels { get; private set; } = new();
        public bool HadStore { get; private set; } = false;

        public void OnSetUp()
        {
            _weaponDatas = _userProfile.BomberDatas;
            _weaponConfigs = _bomberConfig.Weapons;
            HadStore = _weaponDatas != null;

            if (!HadStore) return;
            UpdateWeaponIndexMaxCurrent();
            SetWeaponViewModels();
        }

        private void UpdateWeaponIndexMaxCurrent()
        {
            var weaponLargest = _weaponDatas[_weaponDatas.Count - 1];
            _weaponIndexMaxCurrent = _bomberConfig.GetWeaponIndex(weaponLargest.WeaponId);
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
            var weaponInfo = _bomberConfig.GetWeaponInfo(weaponData.WeaponId);

            // Update state
            weaponViewModel.State.Value = GetWeaponState(weaponData.WeaponId);

            // Update unlockFee
            weaponViewModel.UnlockFee = weaponInfo.UnlockFee;

            // Update reload fee
            var levelUpgradeInfo = weaponInfo.GetLevelUpgradeInfo(weaponData.LevelUpgradeId);
            var bulletRemain = _userProfile.GetWeaponData(weaponData.WeaponId).Quatity;
            var maxBullet = weaponInfo.MaxBullet;
            var reloadFee = levelUpgradeInfo.ReloadFee;
            weaponViewModel.ReloadFee = (reloadFee * bulletRemain) / maxBullet;

            // Update level upgrade fee
            var indexLevelUpgradeCurrent = weaponInfo.GetLevelUpgardeIndex(weaponData.LevelUpgradeId);
            var indexLevelUpgardeMax = weaponInfo.LevelUpgrades.Count - 1;
            if (indexLevelUpgradeCurrent != indexLevelUpgardeMax)
            {
                var levelUpgradeNextInfo = weaponInfo.LevelUpgrades[indexLevelUpgradeCurrent++];
                weaponViewModel.LevelUpgradeFee.Value = levelUpgradeNextInfo.LevelUpFee;

                // Update level upgrade id
                weaponViewModel.LevelUpgradeIdsPassed.Add(levelUpgradeNextInfo.Id);
            }
            else weaponViewModel.LevelUpgradeFee.Value = _levelUpgardeFeeDefault;
        }

        public WeaponState GetWeaponState(string weaponId)
        {
            var weaponIndex = _bomberConfig.GetWeaponIndex(weaponId);

            if (weaponIndex <= _weaponIndexMaxCurrent) return WeaponState.AlreadyHave;
            if (weaponIndex == _weaponIndexMaxCurrent++) return WeaponState.CanUnlock;
            else return WeaponState.CanNotUnlock;
        }

        public void UnlockNewWeapon(string weaponId)
        {
            var weaponViewModel = WeaponWiewModels[weaponId];
            var fee = weaponViewModel.UnlockFee;
            bool result = _coinControllerSystem.PurchaseItem(fee);

            if (result)
            {
                var newWeaponData = new WeaponData
                {
                    WeaponId = weaponId,
                    LevelUpgradeId = LevelUpgradeKey.LevelUpgrade_Default,
                };
                _weaponDatas.Add(newWeaponData);
                _userProfile.Save();

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

            var levelUpgradeFee = weaponModel.LevelUpgradeFee.Value;
            var result = _coinControllerSystem.PurchaseItem(levelUpgradeFee);
            if (result)
            {
                var weaponConfig = _bomberConfig.GetWeaponInfo(weaponId);
                var levelInfoIndexCurrent = weaponConfig.GetLevelUpgardeIndex(weaponModel.LevelUpgradeIdsPassed[weaponModel.LevelUpgradeIdsPassed.Count - 1]);
                var levelNextId = weaponConfig.LevelUpgrades[levelInfoIndexCurrent++].Id;

                weaponModel.LevelUpgradeIdsPassed.Add(levelNextId);
                var weaponData = _userProfile.GetWeaponData(weaponId);
                weaponData.LevelUpgradeId = levelNextId;

                UpdateWeaponViewModel(weaponData);
                _userProfile.Save();
            }
            else Debug.Log("Not enough money!");
        }

        public bool IsHandlerSystem(string weaponId)
        {
            var baseWeapon = StringUtils.GetBaseName(weaponId);
            var baseWeaponSystem = StringUtils.GetBaseName(_weaponDatas[0].WeaponId);

            return baseWeaponSystem == baseWeapon;
        }
    }
}