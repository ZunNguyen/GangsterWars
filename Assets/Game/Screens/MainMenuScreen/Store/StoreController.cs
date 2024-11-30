using Cysharp.Threading.Tasks;
using Sources.DataBaseSystem;
using Sources.DataBaseSystem.Leader;
using Sources.GameData;
using Sources.GamePlaySystem.MainMenuGame;
using Sources.Utils.Singleton;
using UniRx;
using UnityEngine;

namespace Game.Screens.MainMenuScreen
{
    public class StoreController : MonoBehaviour
    {
        private DataBase _dataBase => Locator<DataBase>.Instance;
        private LeaderConfig _leaderConfig => _dataBase.GetConfig<LeaderConfig>();
        private BomberConfig _bomberConfig => _dataBase.GetConfig<BomberConfig>();
        private ShieldConfig _shieldConfig => _dataBase.GetConfig<ShieldConfig>();

        private StoreSystem _storeSystem => Locator<StoreSystem>.Instance;

        [SerializeField] private StoreWeaponHandler _storeLeaderHandler;
        [SerializeField] private StoreWeaponHandler _storeBomberHandler;
        [SerializeField] private StoreWeaponHandler _storeShieldHandler;

        public void OnSetUp()
        {
            _storeLeaderHandler.OnSetUp(_leaderConfig.GetAllWeapons());
            _storeLeaderHandler.SetState(TabState.TabGun);

            _storeShieldHandler.OnSetUp(_shieldConfig.GetAllWeapons());
            _storeShieldHandler.SetState(TabState.TabShield);

            _storeSystem.OnpenBomberStore.Subscribe(SubOpenBomberStore).AddTo(this);
        }

        private void SubOpenBomberStore(bool isOpened)
        {
            if (!isOpened) return;

            _storeBomberHandler.OnSetUp(_bomberConfig.GetAllWeapons());
            _storeBomberHandler.SetState(TabState.TabBom);
        }
    }
}