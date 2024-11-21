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
        public UserRecieveDamageHandler UserRecieveDamageHandler = new();
        public EnemiesController EnemiesController = new();

        public string WaveIdCurrent { get; private set; }

        public override async UniTask Init()
        {
            
        }

        public async void OnSetUp(string waveId)
        {
            WaveIdCurrent = waveId;
            UserRecieveDamageHandler.OnSetUp();
            EnemiesController.OnSetUp(waveId);
            SpawnEnemiesHandler = new();
            SpawnEnemiesHandler.OnSetUp(waveId);

            await UniTask.Delay(2000);
            StartBattle();
        }

        public void StartBattle()
        {
            SpawnEnemiesHandler.SpawnEnemies();
        }
    }
}