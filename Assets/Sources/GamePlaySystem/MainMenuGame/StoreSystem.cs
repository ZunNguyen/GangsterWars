using Cysharp.Threading.Tasks;
using Sources.DataBaseSystem;
using Sources.GameData;
using Sources.SystemService;
using Sources.Utils.Singleton;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace Sources.GamePlaySystem.MainMenuGame
{
    public class InitStoreSystemService : InitSystemService<StoreSystem> { }

    public enum WeaponState
    {
        AlreadyHave,
        CanUnlock,
        CanNotUnlock
    }

    public class StoreSystem : BaseSystem
    {
        private GameData.GameData _gameData => Locator<GameData.GameData>.Instance;
        private StoreProfile _storeProfile => _gameData.GetProfileData<StoreProfile>();
        private UserProfile _userProfile => _gameData.GetProfileData<UserProfile>();

        private DataBase _dataBase => Locator<DataBase>.Instance;
        private StoreConfig _storeConfig => _dataBase.GetConfig<StoreConfig>();

        public StoreWeaponHandler StoreLeaderWeaponHandler = new();
        public StoreWeaponHandler StoreBomberWeaponHandler = new();

        public override async UniTask Init()
        {
            CheckLeaderData();
            CheckBomberData();
            OnSetUp();
        }

        private void CheckLeaderData()
        {
            if (_storeProfile.LeaderWeapons == null)
            {
                _storeProfile.SetStoreLeaderDefault();
            }
        }

        private void CheckBomberData()
        {
            if (_storeProfile.BomberWeapons == null && _userProfile.IsActiveBomber)
            {
                _storeProfile.SetStoreBomberDefault();
            }
        }

        private void OnSetUp()
        {
            StoreLeaderWeaponHandler.OnSetUp(_storeProfile.LeaderWeapons, _storeConfig.LeaderWeapons);
            StoreBomberWeaponHandler.OnSetUp(_storeProfile.BomberWeapons, _storeConfig.BomberWeapons);
        }

        public StoreWeaponHandler GetWeaponHandlerSystem(string weaponId)
        {
            if (StoreLeaderWeaponHandler.IsHandlerSystem(weaponId)) return StoreLeaderWeaponHandler;
            if (StoreBomberWeaponHandler.IsHandlerSystem(weaponId)) return StoreBomberWeaponHandler;
            else return null;
        }
    }
}