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

        public override async UniTask Init()
        {
            SpawnEnemiesHandler = new SpawnEnemiesHandler();

            UserRecieveDamageHandler = new UserRecieveDamageHandler();
            UserRecieveDamageHandler.OnSetUp();

            EnemiesController = new EnemiesController();
        }

        public void StartBattle()
        {
            SpawnEnemiesHandler.SpawnEnemies();
        }

        public void SetWaveId(string waveId)
        {
            SpawnEnemiesHandler.SetWaveId(waveId);
        }
    }
}