using Sources.DataBaseSystem;
using Sources.DataBaseSystem.Leader;
using Sources.Extension;
using Sources.GameData;
using Sources.GamePlaySystem.CoinController;
using Sources.GamePlaySystem.MainMenuGame.Store;
using Sources.Utils.Singleton;
using Sources.Utils.String;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

namespace Sources.GamePlaySystem.MainMenuGame
{
    public class LeaderStoreHandler : StoreHandlerBase
    {
        private GameData.GameData _gameData => Locator<GameData.GameData>.Instance;
        private UserProfile _userProfile => _gameData.GetProfileData<UserProfile>();

        private DataBase _dataBase => Locator<DataBase>.Instance;
        private LeaderConfig _leaderConfig => _dataBase.GetConfig<LeaderConfig>();

        protected override void SetData()
        {
            _weaponConfig = _leaderConfig;

            var weaponDatas = _userProfile.LeaderDatas;
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

            var weaponInfo = _weaponConfig.GetWeaponInfo(weaponId) as LeaderWeaponInfo;

            if (weaponInfo.InfinityBullet) return;

            var levelUpgradeInfo = weaponInfo.GetLevelUpgradeInfo(levelUpgradeId);
            var weaponDataProfile = _userProfile.GetWeaponBaseData(weaponId) as WeaponData;
            var bulletRemain = weaponDataProfile.Quatity;
            var maxBullet = weaponInfo.MaxBullet;
            var reloadFee = levelUpgradeInfo.ReloadFee;

            var reloadFeeCurrent = (maxBullet - bulletRemain) * (reloadFee / maxBullet);
            weaponViewModel.ReloadFee.Value = reloadFeeCurrent;
        }

        protected override void SaveNewData(string weaponId, string levelUpgradeId)
        {
            var newWeaponData = new BaseData
            {
                Id = weaponId,
                LevelUpgradeId = levelUpgradeId,
            };
            _weaponDatas.Add(newWeaponData);

            var weaponInfo = _leaderConfig.GetWeaponInfo(weaponId) as LeaderWeaponInfo;
            var newWeaponDataProfile = new WeaponData
            {
                Id = weaponId,
                LevelUpgradeId = levelUpgradeId,
                Quatity = weaponInfo.MaxBullet
            };
            _userProfile.LeaderDatas.Add(newWeaponDataProfile);

            _userProfile.Save();
        }

        public override ResultBuyItem OnReloadWeapon(string weaponId)
        {
            var weaponModel = WeaponWiewModels[weaponId];

            var result = _coinControllerSystem.PurchaseItem(weaponModel.ReloadFee.Value);
            if (result)
            {
                weaponModel.ReloadFee.Value = 0;

                var weaponConfig = _weaponConfig.GetWeaponInfo(weaponId) as LeaderWeaponInfo;
                var weaponData = _userProfile.GetWeaponBaseData(weaponId) as WeaponData;
                weaponData.Quatity = weaponConfig.MaxBullet;
                weaponModel.WeaponValue.Value = weaponConfig.MaxBullet.ToString();
                _userProfile.Save();

                return ResultBuyItem.Success;
            }
            else return ResultBuyItem.Fail;
        }
    }
}