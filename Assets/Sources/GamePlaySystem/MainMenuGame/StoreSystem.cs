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
            LeaderStoreHandler.OnSetUp();
            BomberStoreHandler.OnSetUp();
        }

        public StoreHandlerBase GetWeaponHandlerSystem(string weaponId)
        {
            if (LeaderStoreHandler.IsHandlerSystem(weaponId)) return LeaderStoreHandler;
            if (BomberStoreHandler.IsHandlerSystem(weaponId)) return BomberStoreHandler;
            else return null;
        }
    }
}