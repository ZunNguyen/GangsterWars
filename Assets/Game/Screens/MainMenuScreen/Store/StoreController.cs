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

        [SerializeField] private StoreWeaponHandler _storeLeaderHandler;
        [SerializeField] private StoreWeaponHandler _storeBomberHandler;
        [SerializeField] private StoreWeaponHandler _storeShieldHandler;

        public void OnSetUp()
        {
            //_storeLeaderHandler.OnSetUp(_leaderConfig.Weapons);
            //_storeLeaderHandler.SetState(TabState.TabGun);

            //_storeBomberHandler.OnSetUp(_bomberConfig.Weapons);
            //_storeBomberHandler.SetState(TabState.TabBom);

            //_storeShieldHandler.SetState(TabState.TabShield);
        }
    }
}