using Cysharp.Threading.Tasks;
using Sources.DataBaseSystem;
using Sources.GameData;
using Sources.SystemService;
using Sources.Utils.Singleton;

namespace Sources.GamePlaySystem.Character
{
    public class InitBomberSystemService : InitSystemService<BomberSystem> { };

    public class BomberSystem : BaseSystem
    {
        private const float _timeReload = 5f;

        private GameData.GameData _gameData => Locator<GameData.GameData>.Instance;
        private UserProfile _userProfile => _gameData.GetProfileData<UserProfile>();

        private DataBase _dataBase => Locator<DataBase>.Instance;
        private BomberConfig _bomberConfig => _dataBase.GetConfig<BomberConfig>();

        public ReloadTimeHandler ReloadTimeHandler = new();
        public WeaponHandler BomHandler = new ();

        public override async UniTask Init(){}

        public void OnSetUp()
        {
            if (_userProfile.BomberDatas == null) return;

            ReloadTimeHandler.OnSetUp(_timeReload);
            BomHandler.OnSetUp(_userProfile.BomberDatas, ReloadTimeHandler, _bomberConfig);
        }
    }
}