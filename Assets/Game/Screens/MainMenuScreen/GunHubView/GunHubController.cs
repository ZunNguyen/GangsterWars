using Cysharp.Threading.Tasks;
using Sources.DataBaseSystem;
using Sources.DataBaseSystem.Leader;
using Sources.GamePlaySystem.MainMenuGame;
using Sources.SpawnerSystem;
using Sources.Utils.Singleton;
using UniRx;
using UnityEngine;

namespace Game.Screens.MainMenuScreen
{
    public class GunHubController : MonoBehaviour
    {
        private DataBase _dataBase => Locator<DataBase>.Instance;
        private LeaderConfig _leaderConfig => _dataBase.GetConfig<LeaderConfig>();
        private BomberConfig _bomberConfig => _dataBase.GetConfig<BomberConfig>();
        private SniperConfig _sniperConfig => _dataBase.GetConfig<SniperConfig>();
        private ShieldConfig _shieldConfig => _dataBase.GetConfig<ShieldConfig>();

        private SpawnerManager _spawnerManager => Locator<SpawnerManager>.Instance;
        private StoreSystem _storeSystem => Locator<StoreSystem>.Instance;

        [SerializeField] private GunHubViewHandler _leaderHub;
        [SerializeField] private GunHubViewHandler _bomberHub;
        [SerializeField] private GunHubViewHandler _sniperHub;
        [SerializeField] private GunHubViewHandler _shieldHub;

        private void Awake()
        {
            _leaderHub.OnSetUp(TabState.TabGun, _leaderConfig.GetAllWeapons());
            _shieldHub.OnSetUp(TabState.TabShield, _shieldConfig.GetAllWeapons());

            _storeSystem.OpenBomberStore.Subscribe(SetGunHubBomber).AddTo(this);
            _storeSystem.OpenSniperStore.Subscribe(SetGunHubSniper).AddTo(this);
        }

        private void SetGunHubBomber(bool isOpen)
        {
            if (isOpen) _bomberHub.OnSetUp(TabState.TabBom, _bomberConfig.GetAllWeapons());
        }

        private void SetGunHubSniper(bool isOpen)
        {
            if (isOpen) _sniperHub.OnSetUp(TabState.TabSniper, _sniperConfig.GetAllWeapons());
        }
    }
}