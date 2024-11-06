using Sources.DataBaseSystem.Leader;
using Sources.DataBaseSystem;
using Sources.Extension;
using Sources.GameData;
using Sources.GamePlaySystem.CoinController;
using Sources.GamePlaySystem.MainMenuGame.Store;
using Sources.Utils.Singleton;
using System.Collections.Generic;
using UnityEngine;
using Sources.Utils.String;

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
            _weaponDatas = _userProfile.BomberDatas;
        }
    }
}