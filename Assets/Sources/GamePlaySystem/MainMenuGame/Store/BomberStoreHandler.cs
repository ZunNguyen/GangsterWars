using Sources.DataBaseSystem;
using Sources.Extension;
using Sources.GameData;
using Sources.GamePlaySystem.MainMenuGame.Store;
using Sources.Utils.Singleton;

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

            var reloadFeeCurrent = (maxBullet - bulletRemain) * (reloadFee / maxBullet);
            weaponViewModel.ReloadFee.Value = reloadFeeCurrent;
        }

        protected override void SaveNewData(string weaponId, string levelUpgradeId)
        {
            var newWeaponData = new WeaponData
            {
                Id = weaponId,
                LevelUpgradeId = LevelUpgradeKey.LEVELUPGRADE_DEFAULT,
            };
            _weaponDatas.Add(newWeaponData);

            var weaponInfo = _bomberConfig.GetWeaponInfo(weaponId) as BomberWeaponInfo;
            var newWeaponDataProfile = new WeaponData
            {
                Id = weaponId,
                LevelUpgradeId = levelUpgradeId,
                Quatity = weaponInfo.MaxBullet
            };
            _userProfile.BomberDatas.Add(newWeaponDataProfile);

            _userProfile.Save();
        }

        public override ResultBuyItem OnReloadWeapon(string weaponId)
        {
            var weaponModel = WeaponWiewModels[weaponId];

            var result = _coinControllerSystem.PurchaseItem(weaponModel.ReloadFee.Value);
            if (result)
            {
                weaponModel.ReloadFee.Value = 0;

                var weaponConfig = _weaponConfig.GetWeaponInfo(weaponId) as BomberWeaponInfo;
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