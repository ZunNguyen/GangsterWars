using Cysharp.Threading.Tasks;
using Sources.Services;
using Sources.Utils.Singleton;

namespace Sources.SaveGame
{
    public class InitGameDataService : Service
    {
        public override async UniTask<IService.Result> Execute()
        {
            GameData.GameData gameDataSave = new GameData.GameData();
            Locator<GameData.GameData>.Set(gameDataSave);

            return IService.Result.Success;
        }
    }
}