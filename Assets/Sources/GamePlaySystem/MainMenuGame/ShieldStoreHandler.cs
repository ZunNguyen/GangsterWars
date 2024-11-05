using Sources.DataBaseSystem;
using Sources.Extension;
using Sources.GameData;
using Sources.GamePlaySystem.CoinController;
using Sources.Utils.Singleton;
using Sources.Utils.String;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace Sources.GamePlaySystem.MainMenuGame.Store
{
    public class ShieldViewModel
    {
        public ReactiveProperty<ItemState> State = new();
        public ReactiveProperty<int> LevelUpgradeFee = new(0);
        public List<string> LevelUpgradeIdsPassed = new();
        public int UnlockFee;
        public int ReloadFee;
        public bool IsChosed;
    }

    public class ShieldStoreHandler
    {
        private const int _levelUpgardeFeeDefault = 0;

        private GameData.GameData _gameData => Locator<GameData.GameData>.Instance;
        private UserProfile _userProfile => _gameData.GetProfileData<UserProfile>();

        private DataBase _dataBase => Locator<DataBase>.Instance;
        private ShieldConfig _shieldConfig => _dataBase.GetConfig<ShieldConfig>();

        private CoinControllerSystem _coinControllerSystem => Locator<CoinControllerSystem>.Instance;

        private List<ShieldData> _shieldDatas;
        private List<ShieldInfo> _shieldConfigs;
        private int _shieldIndexMaxCurrent;

        public Dictionary<string, WeaponViewModel> ShieldWiewModels { get; private set; } = new();
        public bool HadStore { get; private set; } = false;

        public void OnSetUp(DataBaseConfig config)
        {
            _shieldDatas = _userProfile.ShieldDatas;
            _shieldConfigs = _shieldConfig.ShieldInfos;

            HadStore = _shieldDatas != null;

            UpdateShieldIndexMaxCurrent();
            SetShieldViewModels();
        }

        private void UpdateShieldIndexMaxCurrent()
        {
            if (!HadStore) return;
            var shieldLargest = _shieldDatas[_shieldDatas.Count - 1];
            _shieldIndexMaxCurrent = _shieldConfig.GetShieldIndex(shieldLargest.ShieldId);
        }

        private void SetShieldViewModels()
        {
            foreach (var shieldConfig in _shieldConfigs)
            {
                var newShieldViewModel = new WeaponViewModel();
                ShieldWiewModels.Add(shieldConfig.Id, newShieldViewModel);
                var shieldData = new ShieldData
                {
                    ShieldId = shieldConfig.Id,
                    LevelUpgradeId = shieldConfig.LevelUpgrades[0].Id,
                    IsChosed = true,
                };
                UpdateShieldViewModel(shieldData);
            }

            for (int i = 0; i < _shieldDatas.Count; i++)
            {
                UpdateShieldViewModel(_shieldDatas[i]);
            }
        }

        private void UpdateShieldViewModel(ShieldData shieldData)
        {
            var shieldViewModel = ShieldWiewModels[shieldData.ShieldId];
            var shieldInfo = _shieldConfig.GetShieldInfo(shieldData.ShieldId);

            // Update state
            shieldViewModel.State.Value = GetItemState(shieldData.ShieldId);

            // Update unlockFee
            shieldViewModel.UnlockFee = shieldInfo.UnlockFee;

            // Update reload fee
            if (shieldViewModel.State.Value == ItemState.AlreadyHave)
            {
                //var levelUpgradeInfo = shieldInfo.GetLevelUpgradeInfo(shieldData.LevelUpgradeId);
                //var weaponDataProfile = _userProfile.GetWeaponData(shieldData.WeaponId);
                //var bulletRemain = weaponDataProfile.Quatity;
                //var maxBullet = shieldInfo.MaxBullet;
                //var reloadFee = levelUpgradeInfo.ReloadFee;
                //shieldViewModel.ReloadFee = (reloadFee * bulletRemain) / maxBullet;
            }

            // Update level upgrade fee
            var indexLevelUpgradeCurrent = shieldInfo.GetLevelUpgradeIndex(shieldData.LevelUpgradeId);
            var indexLevelUpgardeMax = shieldInfo.LevelUpgrades.Count - 1;
            if (indexLevelUpgradeCurrent != indexLevelUpgardeMax)
            {
                var levelUpgradeNextInfo = shieldInfo.LevelUpgrades[indexLevelUpgradeCurrent + 1];
                shieldViewModel.LevelUpgradeFee.Value = levelUpgradeNextInfo.LevelUpFee;
            }
            else shieldViewModel.LevelUpgradeFee.Value = _levelUpgardeFeeDefault;

            // Update level upgrade id
            for (int i = 0; i <= indexLevelUpgradeCurrent; i++)
            {
                var levelUpgrdePassed = shieldInfo.LevelUpgrades[i].Id;
                if (!shieldViewModel.LevelUpgradeIdsPassed.Contains(levelUpgrdePassed))
                {
                    shieldViewModel.LevelUpgradeIdsPassed.Add(shieldInfo.LevelUpgrades[i].Id);
                }
            }
        }

        public ItemState GetItemState(string weaponId)
        {
            var shieldIndex = _shieldConfig.GetShieldIndex(weaponId);

            if (shieldIndex <= _shieldIndexMaxCurrent) return ItemState.AlreadyHave;
            if (shieldIndex == _shieldIndexMaxCurrent + 1) return ItemState.CanUnlock;
            else return ItemState.CanNotUnlock;
        }

        public void UnlockNewShield(string shieldId)
        {
            var shieldViewModel = ShieldWiewModels[shieldId];
            var fee = shieldViewModel.UnlockFee;
            bool result = _coinControllerSystem.PurchaseItem(fee);

            if (result)
            {
                var newShieldData = new ShieldData
                {
                    ShieldId = shieldId,
                    LevelUpgradeId = LevelUpgradeKey.LevelUpgrade_Default,
                    IsChosed = true,
                };
                _shieldDatas.Add(newShieldData);
                _userProfile.Save();

                UpdateShieldIndexMaxCurrent();
                foreach (var shieldConfig in _shieldConfigs)
                {
                    var shieldData = new ShieldData
                    {
                        ShieldId = shieldConfig.Id,
                        LevelUpgradeId = shieldConfig.LevelUpgrades[0].Id,
                    };
                    UpdateShieldViewModel(shieldData);
                }

                for (int i = 0; i < _shieldDatas.Count; i++)
                {
                    UpdateShieldViewModel(_shieldDatas[i]);
                }

                foreach (var shieldModel in ShieldWiewModels)
                {
                    Debug.Log($"{shieldModel.Value.State}");
                }

                Debug.Log($"Unlock {shieldId} successfully");
            }
            else Debug.Log("Not enough money!");
        }

        public void UpgradeNewLevelShield(string weaponId)
        {
            var shieldModel = ShieldWiewModels[weaponId];
            if (shieldModel.LevelUpgradeFee.Value == _levelUpgardeFeeDefault)
            {
                Debug.Log($"Level Upgrade {shieldModel.LevelUpgradeFee.Value} is max");
                return;
            }

            var levelUpgradeFee = shieldModel.LevelUpgradeFee.Value;
            var result = _coinControllerSystem.PurchaseItem(levelUpgradeFee);
            if (result)
            {
                var shieldConfig = _shieldConfig.GetShieldInfo(weaponId);
                var levelInfoIndexCurrent = shieldConfig.GetLevelUpgradeIndex(shieldModel.LevelUpgradeIdsPassed[shieldModel.LevelUpgradeIdsPassed.Count - 1]);
                var levelNextId = shieldConfig.LevelUpgrades[++levelInfoIndexCurrent].Id;

                shieldModel.LevelUpgradeIdsPassed.Add(levelNextId);
                var shieldData = _userProfile.GetShieldData(weaponId);
                shieldData.LevelUpgradeId = levelNextId;

                UpdateShieldViewModel(shieldData);
                _userProfile.Save();
            }
            else Debug.Log("Not enough money!");
        }

        public bool IsHandlerSystem(string weaponId)
        {
            var baseWeapon = StringUtils.GetBaseName(weaponId);
            var baseWeaponSystem = StringUtils.GetBaseName(_shieldDatas[0].ShieldId);

            return baseWeaponSystem == baseWeapon;
        }
    }
}