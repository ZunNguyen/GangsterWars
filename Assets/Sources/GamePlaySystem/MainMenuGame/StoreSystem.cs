using Cysharp.Threading.Tasks;
using Sources.DataBaseSystem;
using Sources.GameData;
using Sources.GamePlaySystem.MainMenuGame.Store;
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
        private UserProfile _userProfile => _gameData.GetProfileData<UserProfile>();

        public LeaderStoreHandler LeaderStoreHandler = new();
        public BomberStoreHandler BomberStoreHandler = new();

        public override async UniTask Init()
        {
            CheckLeaderData();
            CheckBomberData();
            CheckShieldData();
            OnSetUp();
        }

        private void CheckLeaderData()
        {
            if (_userProfile.LeaderDatas == null)
            {
                _userProfile.SetLeaderDataDefault();
            }
        }

        private void CheckBomberData()
        {
            if (_userProfile.BomberDatas == null)
            {
                _userProfile.SetBomberDataDefault();
            }
        }

        private void CheckShieldData()
        {
            if (_userProfile.ShieldDatas == null)
            {
                _userProfile.SetShieldDataDefault();
            }
        }

        private void OnSetUp()
        {
            LeaderStoreHandler.OnSetUp(_userProfile.LeaderDatas, _storeConfig.LeaderWeapons);
            BomberStoreHandler.OnSetUp(_userProfile.BomberDatas, _storeConfig.BomberWeapons);
        }

        public StoreHandlerBase GetWeaponHandlerSystem(string weaponId)
        {
            if (LeaderStoreHandler.IsHandlerSystem(weaponId)) return LeaderStoreHandler;
            if (BomberStoreHandler.IsHandlerSystem(weaponId)) return BomberStoreHandler;
            else return null;
        }
    }
}