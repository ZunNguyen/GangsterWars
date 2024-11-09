using Sources.Command;
using Sources.DataBaseSystem;
using Sources.GameData;
using Sources.GamePlaySystem.Bomber;
using Sources.GamePlaySystem.CoinController;
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

            return serviceGroup;
        }

        private Service CreateBootstrapLoadingServiceGroup()
        {
            var serviceGroup = new BootstrapLoadingServiceGroup("Bootstrap Loading Service Group");

            serviceGroup.Add(new InitSaveGameDataSystemService());
            serviceGroup.Add(new InitStoreSystemService());
            serviceGroup.Add(new InitLeaderSystemService());
            serviceGroup.Add(new InitBomberSystemService());
            serviceGroup.Add(new InitMainGamePlaySystemService());
            serviceGroup.Add(new InitCoinControllerSystemService());
            serviceGroup.Add(new InitJourneyMapSystemService());

            return serviceGroup;
        }

        private Service CreateAfterBootstrapLoadingScreen()
        {
            var commandServiceGroup = new SequenceServiceCommandGroup("After Bootstrap Loading");

            commandServiceGroup.Add(new LoadMainMenuScenceCommand());
            //commandServiceGroup.Add(new LoadSenceCommand("GamePlay"));

            return commandServiceGroup;
        }
    }
}