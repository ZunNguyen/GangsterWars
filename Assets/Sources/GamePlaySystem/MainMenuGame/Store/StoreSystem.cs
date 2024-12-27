using Cysharp.Threading.Tasks;
using Game.Screens.MainMenuScreen;
using Sources.DataBaseSystem;
using Sources.DataBaseSystem.Leader;
using Sources.Extension;
using Sources.GameData;
using Sources.GamePlaySystem.MainMenuGame.Store;
using Sources.SystemService;
using Sources.Utils.Singleton;
using Sources.Utils.String;
using UniRx;

namespace Sources.GamePlaySystem.MainMenuGame
{
    public class InitStoreSystemService : InitSystemService<StoreSystem> { }

    public enum ItemState
    {
        AlreadyHave,
        CanUnlock,
        CanNotUnlock
    }

    public enum TabState
    {
        TabGun,
        TabBom,
        TabSniper,
        TabShield,
        TabCoin
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
        public SniperStoreHandler SniperStoreHandler { get; private set; }
        public ShieldStoreHandler ShieldStoreHandler { get; private set; }

        public Store.StoreEarnCoinHandler StoreEarnCoinHandler { get; private set; } = new();

        public ReactiveProperty<bool> OpenBomberStore { get; private set; } = new (false);
        public ReactiveProperty<bool> OpenSniperStore { get; private set; } = new (false);

        public ReactiveProperty<TabState> TabCurrent { get; private set; } = new();

        public override async UniTask Init()
        {
            await StoreEarnCoinHandler.OnSetUp();

            OnSetUp();
        }

        public void OnSetUp()
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
            }
            else
            {
                LeaderStoreHandler = new();
                LeaderStoreHandler.OnSetUp();
            }
        }

        private void SetShieldStore()
        {
            if (_userProfile.ShieldDatas == null)
            {
                _userProfile.SetShieldDataDefault();
            }
            else
            {
                ShieldStoreHandler = new();
                ShieldStoreHandler.OnSetUp();
            }
        }

        public void SetBomberStore()
        {
            if (_userProfile.BomberDatas != null)
            {
                BomberStoreHandler = new();
                BomberStoreHandler.OnSetUp();
                OpenBomberStore.Value = true;
            }
        }

        public void SetSniperStore()
        {
            if (_userProfile.SniperDatas != null)
            {
                SniperStoreHandler = new();
                SniperStoreHandler.OnSetUp();
                OpenSniperStore.Value = true;
            }
        }

        public void SetTabCurrent(TabState tabState)
        {
            TabCurrent.Value = tabState;
        }

        public StoreHandlerBase GetWeaponHandlerById(string weaponId)
        {
            var baseId = StringUtils.GetBaseName(weaponId);

            if (baseId == StringUtils.GetBaseName(LeaderKey.GUN_ID_DEFAULT)) return LeaderStoreHandler;
            if (baseId == StringUtils.GetBaseName(BomberKey.BOMBER_ID_DEFAULT)) return BomberStoreHandler;
            if (baseId == StringUtils.GetBaseName(SniperKey.SNIPER_ID_DEFAULT)) return SniperStoreHandler;
            if (baseId == StringUtils.GetBaseName(ShieldKey.SHIELD_ID_DEFAULT)) return ShieldStoreHandler;
            else return null;
        }
    }
}