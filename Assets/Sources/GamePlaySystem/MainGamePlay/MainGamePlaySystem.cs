using Cysharp.Threading.Tasks;
using Sources.DataBaseSystem;
using Sources.GameData;
using Sources.SystemService;
using Sources.Utils.Singleton;
using UniRx;

namespace Sources.GamePlaySystem.MainGamePlay
{
    public class InitMainGamePlaySystemService : InitSystemService<MainGamePlaySystem> { }

    public class MainGamePlaySystem : BaseSystem
    {
        public SpawnEnemiesHandler _spawnEnemiesHandler;

        public override async UniTask Init()
        {
            _spawnEnemiesHandler = new SpawnEnemiesHandler();
        }
    }
}