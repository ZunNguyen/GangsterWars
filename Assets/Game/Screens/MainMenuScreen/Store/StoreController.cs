using Sources.DataBaseSystem;
using Sources.GameData;
using Sources.Utils.Singleton;
using UnityEngine;

namespace Game.Screens.MainMenuScreen
{
    public class StoreController : MonoBehaviour
    {
        private GameData _gameData => Locator<GameData>.Instance;
        private StoreProfile _storeProfile => _gameData.GetProfileData<StoreProfile>();

        private DataBase _dataBase => Locator<DataBase>.Instance;
        private StoreConfig _storeConfig => _dataBase.GetConfig<StoreConfig>();

        [SerializeField] private StoreWeaponHandler _storeLeaderHandler;
        [SerializeField] private StoreWeaponHandler _storeBomberHandler;
        [SerializeField] private StoreWeaponHandler _storeShieldHandler;

        public void OnSetUp()
        {
            _storeLeaderHandler.OnSetUp(_storeProfile.LeaderWeapons, _storeConfig.LeaderWeapons);
            _storeLeaderHandler.SetState(TabState.TabGun);

            _storeBomberHandler.OnSetUp(_storeProfile.BomberWeapons, _storeConfig.BomberWeapons);
            _storeBomberHandler.SetState(TabState.TabBom);

            _storeShieldHandler.SetState(TabState.TabShield);
        }
    }
}