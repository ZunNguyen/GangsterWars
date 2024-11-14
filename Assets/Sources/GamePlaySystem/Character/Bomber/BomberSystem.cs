using Cysharp.Threading.Tasks;
using Sources.GameData;
using Sources.SystemService;
using Sources.Utils.Singleton;

namespace Sources.GamePlaySystem.Character
{
    public class InitBomberSystemService : InitSystemService<BomberSystem> { };

    public class BomberSystem : BaseSystem
    {
        private const float _timeReload = 3f;

        private GameData.GameData _gameData => Locator<GameData.GameData>.Instance;
        private UserProfile _userProfile => _gameData.GetProfileData<UserProfile>();

        public ReloadTimeHandler ReloadTimeHandler = new();
        public WeaponHandler BomHandler = new ();

        public override async UniTask Init()
        {
            ReloadTimeHandler.OnSetUp(_timeReload);
            BomHandler.OnSetUp(_userProfile.BomberDatas, ReloadTimeHandler);
        }
    }
}