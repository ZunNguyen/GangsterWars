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
using UniRx;
using UnityEngine;

namespace Sources.GamePlaySystem.MainMenuGame.Store
{
    public class ShieldViewModel : WeaponViewModel
    {
        public Action<bool> IsChosed;
    }

    public class ShieldStoreHandler : StoreHandlerBase
    {
        private GameData.GameData _gameData => Locator<GameData.GameData>.Instance;
        private UserProfile _userProfile => _gameData.GetProfileData<UserProfile>();

        private DataBase _dataBase => Locator<DataBase>.Instance;
        private ShieldConfig _shieldConfig => _dataBase.GetConfig<ShieldConfig>();

        protected new List<ShieldData> _weaponDatas;
        public new Dictionary<string, ShieldViewModel> WeaponWiewModels { get; private set; } = new();

        protected override void SetData()
        {
            _weaponConfig = _shieldConfig;
            _weaponDatas = _userProfile.ShieldDatas;
        }

        public override void OnSetUp()
        {
            base.OnSetUp();
            ChoseShield(_weaponDatas[0].Id);
        }

        protected override void UpdateReloadFee(BaseData weaponData)
        {
            var weaponViewModel = WeaponWiewModels[weaponData.Id];
            if (weaponViewModel.State.Value != ItemState.AlreadyHave) return;

            var weaponInfo = _weaponConfig.GetWeaponInfo(weaponData.Id) as ShieldWeaponInfo;
            var levelUpgradeInfo = weaponInfo.GetLevelUpgradeInfo(weaponData.LevelUpgradeId);
            var weaponDataProfile = _userProfile.GetWeaponBaseData(weaponData.Id) as ShieldData;
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
    }
}