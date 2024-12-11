using Cysharp.Threading.Tasks;
using Sources.DataBaseSystem;
using Sources.DataBaseSystem.Leader;
using Sources.Extension;
using Sources.GameData;
using Sources.GamePlaySystem.CoinController;
using Sources.Utils.Singleton;
using Sources.Utils.String;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

namespace Sources.GamePlaySystem.MainMenuGame.Store
{
    public enum ResultBuyItem
    {
        Success,
        Fail
    }

    public class WeaponViewModel
    {
        public ReactiveProperty<ItemState> State = new(ItemState.CanNotUnlock);
        public ReactiveProperty<int> LevelUpgradeFee = new(0);
        public ReactiveProperty<int> ReloadFee = new(0);
        public ReactiveProperty<string> WeaponValue = new(); // if weapon -> is count bullet, if shield -> is state
        public ReactiveProperty<bool> IsChosed = new();
        public List<string> LevelUpgradeIdsPassed = new();
        public int UnlockFee;
    }

    public abstract class StoreHandlerBase
    {
        private const int _levelUpgardeFeeDefault = 0;

        private GameData.GameData _gameData => Locator<GameData.GameData>.Instance;
        private UserProfile _userProfile => _gameData.GetProfileData<UserProfile>();

        protected WeaponConfig _weaponConfig;
        protected List<BaseData> _weaponDatas = new();
        protected IEnumerable<WeaponInfoBase> _weaponInfoConfigs;
        
        protected CoinControllerSystem _coinControllerSystem => Locator<CoinControllerSystem>.Instance;
        private int _weaponIndexMaxCurrent;

        public Dictionary<string, WeaponViewModel> WeaponWiewModels { get; private set; } = new();
        public bool HadStore { get; private set; } = false;

        public virtual void OnSetUp()
        {
            WeaponWiewModels.Clear();
            SetData();
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

        protected virtual void SetWeaponViewModels()
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
                UpdateWeaponViewModel(weaponData.Id, weaponData.LevelUpgradeId);
            }

            for (int i = 0; i < _weaponDatas.Count; i++)
            {
                UpdateWeaponViewModel(_weaponDatas[i].Id, _weaponDatas[i].LevelUpgradeId);
            }
        }

        protected virtual void UpdateWeaponViewModel(string weaponId, string levelUpgradeId)
        {
            var weaponViewModel = WeaponWiewModels[weaponId];
            var weaponInfo = _weaponConfig.GetWeaponInfo(weaponId);

            // Update state
            weaponViewModel.State.Value = GetWeaponState(weaponId);

            if (weaponViewModel.State.Value == ItemState.AlreadyHave)
            {
                var weaponData = _userProfile.GetWeaponBaseData(weaponId) as WeaponData;
                weaponViewModel.WeaponValue.Value = weaponData.Quatity.ToString();
            }

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

        protected abstract void UpdateReloadFee(string weaponId, string levelUpgradeId);

        public ItemState GetWeaponState(string weaponId)
        {
            var weaponIndex = _weaponConfig.GetWeaponIndex(weaponId);

            if (weaponIndex <= _weaponIndexMaxCurrent) return ItemState.AlreadyHave;
            if (weaponIndex == _weaponIndexMaxCurrent + 1) return ItemState.CanUnlock;
            else return ItemState.CanNotUnlock;
        }

        public ResultBuyItem UnlockNewWeapon(string weaponId)
        {
            var weaponViewModel = WeaponWiewModels[weaponId];
            var fee = weaponViewModel.UnlockFee;
            bool result = _coinControllerSystem.PurchaseItem(fee);

            if (result)
            {
                SaveNewData(weaponId, LevelUpgradeKey.LEVELUPGRADE_DEFAULT);
                UpdateWeaponIndexMaxCurrent();
                foreach (var weaponConfig in _weaponInfoConfigs)
                {
                    UpdateWeaponViewModel(weaponConfig.Id, weaponConfig.LevelUpgrades[0].Id);
                }

                for (int i = 0; i < _weaponDatas.Count; i++)
                {
                    UpdateWeaponViewModel(_weaponDatas[i].Id, _weaponDatas[i].LevelUpgradeId);
                }

                return ResultBuyItem.Success;
            }
            else return ResultBuyItem.Fail;
        }

        protected abstract void SaveNewData(string weaponId, string levelUpgradeId);

        public ResultBuyItem UpgradeNewLevelWeapon(string weaponId)
        {
            var weaponModel = WeaponWiewModels[weaponId];
            if (weaponModel.LevelUpgradeFee.Value == _levelUpgardeFeeDefault)
            {
                Debug.Log($"Level Upgrade {weaponModel.LevelUpgradeFee.Value} is max");
                return ResultBuyItem.Fail;
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

                UpdateWeaponViewModel(weaponData.Id, weaponData.LevelUpgradeId);
                _userProfile.Save();

                return ResultBuyItem.Success;
            }
            else return ResultBuyItem.Fail;
        }

        public abstract ResultBuyItem OnReloadWeapon(string weaponId);
    }
}