using Sources.DataBaseSystem;
using Sources.DataBaseSystem.Leader;
using Sources.GameData;
using Sources.Utils.Singleton;
using UnityEngine;

namespace Game.Screens.MainMenuScreen
{
    public class StoreController : MonoBehaviour
    {
        private DataBase _dataBase => Locator<DataBase>.Instance;
        private LeaderConfig _leaderConfig => _dataBase.GetConfig<LeaderConfig>();
        private BomberConfig _bomberConfig => _dataBase.GetConfig<BomberConfig>();
        private ShieldConfig _shieldConfig => _dataBase.GetConfig<ShieldConfig>();

        [SerializeField] private StoreWeaponHandler _storeLeaderHandler;
        [SerializeField] private StoreWeaponHandler _storeBomberHandler;
        [SerializeField] private StoreWeaponHandler _storeShieldHandler;

        public void OnSetUp()
        {
            _storeLeaderHandler.OnSetUp(_leaderConfig.GetAllWeapons());
            _storeLeaderHandler.SetState(TabState.TabGun);

            _storeBomberHandler.OnSetUp(_bomberConfig.GetAllWeapons());
            _storeBomberHandler.SetState(TabState.TabBom);

            _storeShieldHandler.OnSetUp(_shieldConfig.GetAllWeapons());
            _storeShieldHandler.SetState(TabState.TabShield);
        }
    }
}