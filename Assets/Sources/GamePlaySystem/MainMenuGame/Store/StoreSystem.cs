using Cysharp.Threading.Tasks;
using Sources.DataBaseSystem;
using Sources.DataBaseSystem.Leader;
using Sources.Extension;
using Sources.GameData;
using Sources.GamePlaySystem.MainMenuGame.Store;
using Sources.SystemService;
using Sources.Utils.Singleton;
using Sources.Utils.String;
using System;
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

        public ReactiveProperty<bool> OnpenBomberStore { get; private set; } = new (false);

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
                OnpenBomberStore.Value = true;
            }
        }

        public void SetSniperStore()
        {
            if (_userProfile.SniperDatas != null)
            {

            }
        }

        public StoreHandlerBase GetWeaponHandlerSystem(string weaponId)
        {
            var baseId = StringUtils.GetBaseName(weaponId);

            if (baseId == StringUtils.GetBaseName(LeaderKey.GUN_ID_DEFAULT)) return LeaderStoreHandler;
            if (baseId == StringUtils.GetBaseName(BomberKey.BOMBER_ID_DEFAULT)) return BomberStoreHandler;
            if (baseId == StringUtils.GetBaseName(ShieldKey.SHIELD_ID_DEFAULT)) return ShieldStoreHandler;
            else return null;
        }
    }
}