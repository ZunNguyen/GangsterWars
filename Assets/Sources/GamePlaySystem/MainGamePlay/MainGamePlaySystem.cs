using Cysharp.Threading.Tasks;
using Sources.DataBaseSystem;
using Sources.GameData;
using Sources.GamePlaySystem.MainGamePlay.Enemies;
using Sources.SystemService;
using Sources.Utils.Singleton;
using System;
using UniRx;

namespace Sources.GamePlaySystem.MainGamePlay
{
    public class InitMainGamePlaySystemService : InitSystemService<MainGamePlaySystem> { }

    public class MainGamePlaySystem : BaseSystem
    {
        public SpawnEnemiesHandler SpawnEnemiesHandler;
        public UserRecieveDamageHandler UserRecieveDamageHandler;
        public EnemiesController EnemiesController;

        public Action StartBattel;

        public override async UniTask Init()
        {
            SpawnEnemiesHandler = new SpawnEnemiesHandler();

            UserRecieveDamageHandler = new UserRecieveDamageHandler();
            UserRecieveDamageHandler.OnSetUp();

            EnemiesController = new EnemiesController();
        }

        public async void SetWaveId(string waveId)
        {
            SpawnEnemiesHandler.SetWaveId(waveId);

            await UniTask.Delay(2000);
            StartBattle();
        }

        public void StartBattle()
        {
            StartBattel?.Invoke();
            SpawnEnemiesHandler.SpawnEnemies();
        }
    }
}