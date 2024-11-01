using Cysharp.Threading.Tasks;
using Sources.SystemService;
using Sources.Utils.Singleton;

namespace Sources.GameData
{
    public class InitSaveGameDataSystemService : InitSystemService<SaveGameDataSystem> { };

    public class SaveGameDataSystem : BaseSystem
    {
        private GameData _gameData => Locator<GameData>.Instance;
        private UserProfile _userProfile => _gameData.GetProfileData<UserProfile>();

        public override async UniTask Init()
        {
            
        }

        private void OnApplicationQuit()
        {
            _userProfile.Save();
        }
    }
}