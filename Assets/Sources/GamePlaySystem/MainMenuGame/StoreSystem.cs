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

    public class StoreSystem : BaseSystem
    {
        private GameData.GameData _gameData => Locator<GameData.GameData>.Instance;
        private StoreProfile _storeProfile => _gameData.GetProfileData<StoreProfile>();
        private DataBase _dataBase => Locator<DataBase>.Instance;
        private StoreConfig _storeConfig => _dataBase.GetConfig<StoreConfig>();

        public ReactiveProperty<string> LeaderStoreIds { get; private set; }
        public bool HadStoreBomber { get; private set; } = false;

        public override async UniTask Init()
        {
            HadStoreBomber = _storeProfile.BomberWeapon != null;
        }
    }
}