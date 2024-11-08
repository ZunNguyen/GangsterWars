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
        private GameData.GameData _gameData => Locator<GameData.GameData>.Instance;
        private UserProfile _userProfile => _gameData.GetProfileData<UserProfile>();

        private DataBase _dataBase => Locator<DataBase>.Instance;
        private BomberConfig _bomberConfig => _dataBase.GetConfig<BomberConfig>();

        protected override void SetData()
        {
            _weaponConfig = _bomberConfig;

            var weaponDatas = _userProfile.BomberDatas;
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

        protected override void UpdateReloadFee(string weaponId, string levelUpgradeId)
        {
            var weaponViewModel = WeaponWiewModels[weaponId];
            if (weaponViewModel.State.Value != ItemState.AlreadyHave) return;

            var weaponInfo = _weaponConfig.GetWeaponInfo(weaponId) as BomberWeaponInfo;
            var levelUpgradeInfo = weaponInfo.GetLevelUpgradeInfo(levelUpgradeId);
            var weaponDataProfile = _userProfile.GetWeaponBaseData(weaponId) as WeaponData;
            var bulletRemain = weaponDataProfile.Quatity;
            var maxBullet = weaponInfo.MaxBullet;
            var reloadFee = levelUpgradeInfo.ReloadFee;

            weaponViewModel.ReloadFee = (reloadFee * bulletRemain) / maxBullet;
        }

        protected override void SaveNewData(string weaponId, string levelUpgradeId)
        {
            var newWeaponData = new WeaponData
            {
                Id = weaponId,
                LevelUpgradeId = LevelUpgradeKey.LevelUpgrade_Default,
            };
            _weaponDatas.Add(newWeaponData);

            var newWeaponDataProfile = new WeaponData
            {
                Id = weaponId,
                LevelUpgradeId = levelUpgradeId,
                Quatity = 50
            };
            _userProfile.BomberDatas.Add(newWeaponDataProfile);

            _userProfile.Save();
        }
    }
}