using Game.Character.Bomber;
using Sources.DataBaseSystem;
using Sources.DataBaseSystem.Leader;
using Sources.Extension;
using Sources.GameData;
using Sources.GamePlaySystem.CoinController;
using Sources.GamePlaySystem.MainGamePlay;
using Sources.Utils.Singleton;
using Sources.Utils.String;
using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

namespace Sources.GamePlaySystem.MainMenuGame.Store
{
    public class ShieldStoreHandler : StoreHandlerBase
    {
        private GameData.GameData _gameData => Locator<GameData.GameData>.Instance;
        private UserProfile _userProfile => _gameData.GetProfileData<UserProfile>();

        private DataBase _dataBase => Locator<DataBase>.Instance;
        private ShieldConfig _shieldConfig => _dataBase.GetConfig<ShieldConfig>();

        protected override void SetData()
        {
            _weaponConfig = _shieldConfig;

            var weaponDatas = _userProfile.ShieldDatas;
            foreach (var weaponData in weaponDatas)
            {
                var newWeaponData = new BaseData
                {
                    Id = weaponData.Id,
                    LevelUpgradeId = weaponData.LevelUpgradeId,
                };
                _weaponDatas.Add(newWeaponData);
            }
        }

        public override void OnSetUp()
        {
            base.OnSetUp();

            ChoseShield(_userProfile.ShieldDatas[0].Id);
        }

        protected override void SetWeaponViewModels()
        {
            foreach (var weaponConfig in _weaponInfoConfigs)
            {
                var newWeaponViewModel = new WeaponViewModel();
                WeaponWiewModels.Add(weaponConfig.Id, newWeaponViewModel);

                UpdateWeaponViewModel(weaponConfig.Id, weaponConfig.LevelUpgrades[0].Id);
            }

            foreach (var weaponData in _weaponDatas)
            {
                UpdateWeaponViewModel(weaponData.Id, weaponData.LevelUpgradeId);
            }
        }

        protected override void UpdateWeaponViewModel(string weaponId, string levelUpgradeId)
        {
            var weaponViewModel = WeaponWiewModels[weaponId];
            var weaponInfo = _weaponConfig.GetWeaponInfo(weaponId);

            // Update state
            weaponViewModel.State.Value = GetWeaponState(weaponId);

            // Update unlockFee
            weaponViewModel.UnlockFee = weaponInfo.UnlockFee;

            // Update reload fee
            UpdateReloadFee(weaponId, levelUpgradeId);

            // Update level upgrade fee
            var indexLevelUpgradeCurrent = weaponInfo.GetLevelUpgardeIndex(levelUpgradeId);
            var indexLevelUpgardeMax = weaponInfo.LevelUpgrades.Count - 1;
            if (indexLevelUpgradeCurrent != indexLevelUpgardeMax)
            {
                var levelUpgradeNextInfo = weaponInfo.LevelUpgrades[indexLevelUpgradeCurrent + 1];
                weaponViewModel.LevelUpgradeFee.Value = levelUpgradeNextInfo.LevelUpFee;
            }
            else weaponViewModel.LevelUpgradeFee.Value = 0;

            // Update level upgrade id
            for (int i = 0; i <= indexLevelUpgradeCurrent; i++)
            {
                var levelUpgrdePassed = weaponInfo.LevelUpgrades[i].Id;
                if (!weaponViewModel.LevelUpgradeIdsPassed.Contains(levelUpgrdePassed))
                {
                    weaponViewModel.LevelUpgradeIdsPassed.Add(weaponInfo.LevelUpgrades[i].Id);
                }
            }
        }

        protected override void UpdateReloadFee(string weaponId, string levelUpgradeId)
        {
            var weaponViewModel = WeaponWiewModels[weaponId];
            if (weaponViewModel.State.Value != ItemState.AlreadyHave) return;

            var weaponInfo = _weaponConfig.GetWeaponInfo(weaponId) as ShieldWeaponInfo;
            var levelUpgradeInfo = weaponInfo.GetLevelUpgradeInfo(levelUpgradeId);
            var weaponDataProfile = _userProfile.GetWeaponBaseData(weaponId) as ShieldData;
            var factor = GetFactorShieldState(weaponDataProfile.State);
            var reloadFee = levelUpgradeInfo.ReloadFee;

            weaponViewModel.ReloadFee = (int)(reloadFee * factor);
        }

        private float GetFactorShieldState(ShieldState shieldState)
        {
            return (int)shieldState / 100; //100 -> 100%
        }

        public void ChoseShield(string shieldId)
        {
            foreach(var weaponViewModel in WeaponWiewModels)
            {
                if (weaponViewModel.Key == shieldId) weaponViewModel.Value.IsChosed?.Invoke(true);
                else weaponViewModel.Value.IsChosed?.Invoke(false);
            }

            _userProfile.ChoseShield(shieldId);
        }

        protected override void SaveNewData(string weaponId, string levelUpgradeId)
        {
            var newWeaponData = new BaseData
            {
                Id = weaponId,
                LevelUpgradeId = LevelUpgradeKey.LevelUpgrade_Default,
            };
            _weaponDatas.Add(newWeaponData);

            var newWeaponDataProfile = new ShieldData
            {
                Id = weaponId,
                LevelUpgradeId = levelUpgradeId,
                IsChosed = true,
            };
            _userProfile.ShieldDatas.Add(newWeaponDataProfile);

            _userProfile.Save();
        }
    }
}