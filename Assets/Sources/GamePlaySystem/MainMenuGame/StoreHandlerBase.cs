using Cysharp.Threading.Tasks;
using Sources.DataBaseSystem;
using Sources.DataBaseSystem.Leader;
using Sources.Extension;
using Sources.GameData;
using Sources.GamePlaySystem.CoinController;
using Sources.Utils.Singleton;
using Sources.Utils.String;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

namespace Sources.GamePlaySystem.MainMenuGame.Store
{
    public class WeaponViewModel
    {
        public ReactiveProperty<ItemState> State = new();
        public ReactiveProperty<int> LevelUpgradeFee = new(0);
        public List<string> LevelUpgradeIdsPassed = new();
        public int UnlockFee;
        public int ReloadFee;
    }

    public abstract class StoreHandlerBase
    {
        private const int _levelUpgardeFeeDefault = 0;

        private GameData.GameData _gameData => Locator<GameData.GameData>.Instance;
        private UserProfile _userProfile => _gameData.GetProfileData<UserProfile>();

        protected WeaponConfig _weaponConfig;
        protected List<BaseData> _weaponDatas;

        private IEnumerable<WeaponInfoBase> _weaponInfoConfigs;
        private CoinControllerSystem _coinControllerSystem => Locator<CoinControllerSystem>.Instance;
        private int _weaponIndexMaxCurrent;

        public Dictionary<string, WeaponViewModel> WeaponWiewModels { get; private set; } = new();
        public bool HadStore { get; private set; } = false;

        public virtual async void OnSetUp()
        {
            SetData();
            await UniTask.DelayFrame(1);
            _weaponInfoConfigs = _weaponConfig.GetAllWeapons();

            HadStore = _weaponDatas != null;
            if (!HadStore) return;
            UpdateWeaponIndexMaxCurrent();
            SetWeaponViewModels();
        }

        protected abstract void SetData();

        private void UpdateWeaponIndexMaxCurrent()
        {
            var weaponLargest = _weaponDatas[_weaponDatas.Count - 1];
            _weaponIndexMaxCurrent = _weaponConfig.GetWeaponIndex(weaponLargest.Id);
        }

        private void SetWeaponViewModels()
        {
            foreach (var weaponConfig in _weaponInfoConfigs)
            {
                var newWeaponViewModel = new WeaponViewModel();
                WeaponWiewModels.Add(weaponConfig.Id, newWeaponViewModel);
                var weaponData = new BaseData
                {
                    Id = weaponConfig.Id,
                    LevelUpgradeId = weaponConfig.LevelUpgrades[0].Id,
                };
                UpdateWeaponViewModel(weaponData);
            }

            for (int i = 0; i < _weaponDatas.Count; i++)
            {
                UpdateWeaponViewModel(_weaponDatas[i] as WeaponData);
            }
        }

        private void UpdateWeaponViewModel(BaseData weaponData)
        {
            var weaponViewModel = WeaponWiewModels[weaponData.Id];
            var weaponInfo = _weaponConfig.GetWeaponInfo(weaponData.Id);

            // Update state
            weaponViewModel.State.Value = GetWeaponState(weaponData.Id);

            // Update unlockFee
            weaponViewModel.UnlockFee = weaponInfo.UnlockFee;

            // Update reload fee
            UpdateReloadFee(weaponData);

            // Update level upgrade fee
            var indexLevelUpgradeCurrent = weaponInfo.GetLevelUpgardeIndex(weaponData.LevelUpgradeId);
            var indexLevelUpgardeMax = weaponInfo.LevelUpgrades.Count - 1;
            if (indexLevelUpgradeCurrent != indexLevelUpgardeMax)
            {
                var levelUpgradeNextInfo = weaponInfo.LevelUpgrades[indexLevelUpgradeCurrent + 1];
                weaponViewModel.LevelUpgradeFee.Value = levelUpgradeNextInfo.LevelUpFee;
            }
            else weaponViewModel.LevelUpgradeFee.Value = _levelUpgardeFeeDefault;

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

        protected abstract void UpdateReloadFee(BaseData weaponData);

        public ItemState GetWeaponState(string weaponId)
        {
            var weaponIndex = _weaponConfig.GetWeaponIndex(weaponId);

            if (weaponIndex <= _weaponIndexMaxCurrent) return ItemState.AlreadyHave;
            if (weaponIndex == _weaponIndexMaxCurrent + 1) return ItemState.CanUnlock;
            else return ItemState.CanNotUnlock;
        }

        public void UnlockNewWeapon(string weaponId)
        {
            var weaponViewModel = WeaponWiewModels[weaponId];
            var fee = weaponViewModel.UnlockFee;
            bool result = _coinControllerSystem.PurchaseItem(fee);

            if (result)
            {
                var newWeaponData = new BaseData
                {
                    Id = weaponId,
                    LevelUpgradeId = LevelUpgradeKey.LevelUpgrade_Default,
                };
                _weaponDatas.Add(newWeaponData);
                _userProfile.Save();

                UpdateWeaponIndexMaxCurrent();
                foreach (var weaponConfig in _weaponInfoConfigs)
                {
                    var weaponData = new BaseData
                    {
                        Id = weaponConfig.Id,
                        LevelUpgradeId = weaponConfig.LevelUpgrades[0].Id,
                    };
                    UpdateWeaponViewModel(weaponData);
                }

                for (int i = 0; i < _weaponDatas.Count; i++)
                {
                    UpdateWeaponViewModel(_weaponDatas[i] as BaseData);
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
                var weaponConfig = _weaponConfig.GetWeaponInfo(weaponId);
                var levelInfoIndexCurrent = weaponConfig.GetLevelUpgardeIndex(weaponModel.LevelUpgradeIdsPassed[weaponModel.LevelUpgradeIdsPassed.Count - 1]);
                var levelNextId = weaponConfig.LevelUpgrades[++levelInfoIndexCurrent].Id;

                weaponModel.LevelUpgradeIdsPassed.Add(levelNextId);
                var weaponData = _userProfile.GetWeaponBaseData(weaponId);
                weaponData.LevelUpgradeId = levelNextId;

                UpdateWeaponViewModel(weaponData);
                _userProfile.Save();
            }
            else Debug.Log("Not enough money!");
        }

        public bool IsHandlerSystem(string weaponId)
        {
            var baseWeapon = StringUtils.GetBaseName(weaponId);
            var baseWeaponSystem = StringUtils.GetBaseName(_weaponDatas[0].Id);

            return baseWeaponSystem == baseWeapon;
        }
    }
}