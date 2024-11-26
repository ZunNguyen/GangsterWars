using Sources.Audio;
using Sources.Command;
using Sources.DataBaseSystem;
using Sources.GameData;
using Sources.GamePlaySystem.Character;
using Sources.GamePlaySystem.CoinController;
using Sources.GamePlaySystem.GameResult;
using Sources.GamePlaySystem.JourneyMap;
using Sources.GamePlaySystem.Leader;
using Sources.GamePlaySystem.MainGamePlay;
using Sources.GamePlaySystem.MainMenuGame;
using Sources.SaveGame;
using Sources.Services;
using Sources.Services.BootstrapLoadingService;
using Sources.SpawnerSystem;
using Sources.UISystem;
using UnityEngine;

namespace Game.Bootstrap
{
    public class Bootstrap : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private UIData _uiData;
        [SerializeField] private UIManager _uiManagerPrefab;

        [Header("DataBase")]
        [SerializeField] private DataBase _dataBase;

        [Header("Audio Object Instance")]
        [SerializeField] private AudioObjectInstance _audioObjectInstance;

        private async void Start()
        {
            var bootStrapService = CreateBootstrapServiceGroup();
            await bootStrapService.Run();
        }

        private Service CreateBootstrapServiceGroup()
        {
            var serviceGroup = new SequenceServiceGroup("Bootstrap Service Group");

            serviceGroup.Add(CreateEssentialSericeGroup());
            serviceGroup.Add(CreateBootstrapLoadingServiceGroup());
            serviceGroup.Add(CreateAfterBootstrapLoadingScreen());

            return serviceGroup;
        }

        private Service CreateEssentialSericeGroup()
        {
            var serviceGroup = new SequenceServiceGroup("Essential Service Group");

            serviceGroup.Add(new InitUISystemService(_uiData, _uiManagerPrefab));
            serviceGroup.Add(new InitSpawnerManagerService());
            serviceGroup.Add(new InitDataBaseService(_dataBase));
            serviceGroup.Add(new InitGameDataService());
            serviceGroup.Add(new InitEventSystemService());
            serviceGroup.Add(new InitAudioManagerService(_audioObjectInstance));

            return serviceGroup;
        }

        private Service CreateBootstrapLoadingServiceGroup()
        {
            var serviceGroup = new BootstrapLoadingServiceGroup("Bootstrap Loading Service Group");

            serviceGroup.Add(new InitSaveGameDataSystemService());
            serviceGroup.Add(new InitStoreSystemService());
            serviceGroup.Add(new InitCoinControllerSystemService());
            serviceGroup.Add(new InitJourneyMapSystemService());

            // Main game play service
            serviceGroup.Add(new InitMainGamePlaySystemService());
            serviceGroup.Add(new InitLeaderSystemService());
            serviceGroup.Add(new InitBomberSystemService());
            serviceGroup.Add(new InitSniperSystemService());
            serviceGroup.Add(new InitGameResultSystemService());

            return serviceGroup;
        }

        private Service CreateAfterBootstrapLoadingScreen()
        {
            var commandServiceGroup = new SequenceServiceCommandGroup("After Bootstrap Loading");

            commandServiceGroup.Add(new LoadMainMenuScenceCommand());

            return commandServiceGroup;
        }
    }
}