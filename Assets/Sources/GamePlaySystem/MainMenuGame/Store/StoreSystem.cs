using Cysharp.Threading.Tasks;
using Sources.DataBaseSystem;
using Sources.DataBaseSystem.Leader;
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

    public enum ItemState
    {
        AlreadyHave,
        CanUnlock,
        CanNotUnlock
    }

    public class StoreSystem : BaseSystem
    {
        private GameData.GameData _gameData => Locator<GameData.GameData>.Instance;
        private UserProfile _userProfile => _gameData.GetProfileData<UserProfile>();

        private DataBase _dataBase => Locator<DataBase>.Instance;
        private LeaderConfig _leaderConfig => _dataBase.GetConfig<LeaderConfig>();
        private BomberConfig _bomberConfig => _dataBase.GetConfig<BomberConfig>();

        public LeaderStoreHandler LeaderStoreHandler { get; private set; }
        public BomberStoreHandler BomberStoreHandler { get; private set; }
        public ShieldStoreHandler ShieldStoreHandler { get; private set; }

        public override async UniTask Init()
        {
            SetLeaderStore();
            SetBomberStore();
            SetSniperStore();
            SetShieldStore();
        }

        private void SetLeaderStore()
        {
            if (_userProfile.LeaderDatas == null)
            {
                _userProfile.SetLeaderDataDefault();
                LeaderStoreHandler = new();
                LeaderStoreHandler.OnSetUp();
            }
        }

        private void SetBomberStore()
        {
            if (_userProfile.BomberDatas == null)
            {
                _userProfile.SetBomberDataDefault();
                BomberStoreHandler = new();
                BomberStoreHandler.OnSetUp();
            }
        }

        private void SetSniperStore()
        {
            if (_userProfile.SniperDatas == null)
            {
                _userProfile.SetSniperDataDefault();
            }
        }

        private void SetShieldStore()
        {
            if (_userProfile.ShieldDatas == null)
            {
                _userProfile.SetShieldDataDefault();
                ShieldStoreHandler = new();
                ShieldStoreHandler.OnSetUp();
            }
        }

        public StoreHandlerBase GetWeaponHandlerSystem(string weaponId)
        {
            if (LeaderStoreHandler.IsHandlerSystem(weaponId)) return LeaderStoreHandler;
            if (BomberStoreHandler.IsHandlerSystem(weaponId)) return BomberStoreHandler;
            if (ShieldStoreHandler.IsHandlerSystem(weaponId)) return ShieldStoreHandler;
            else return null;
        }
    }
}