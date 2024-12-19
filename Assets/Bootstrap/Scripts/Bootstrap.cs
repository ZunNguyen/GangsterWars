using Sources.AdMob;
using Sources.Audio;
using Sources.Command;
using Sources.DataBaseSystem;
using Sources.FTUE.System;
using Sources.GameData;
using Sources.GamePlaySystem.Character;
using Sources.GamePlaySystem.CoinController;
using Sources.GamePlaySystem.GameResult;
using Sources.GamePlaySystem.JourneyMap;
using Sources.GamePlaySystem.Leader;
using Sources.GamePlaySystem.MainGamePlay;
using Sources.GamePlaySystem.MainMenuGame;
using Sources.Language;
using Sources.SaveGame;
using Sources.Services;
using Sources.Services.BootstrapLoadingService;
using Sources.SpawnerSystem;
using Sources.TimeManager;
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

        [Header("LanguageTable")]
        [SerializeField] private LanguageTable _languageTable;

        [Header("Audio Object Instance")]
        [SerializeField] private AudioData _audioData;
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
            serviceGroup.Add(new InitAudioManagerService(_audioData, _audioObjectInstance));
            serviceGroup.Add(new InitLanaguageTableService(_languageTable));
            serviceGroup.Add(new InitTimeManagerSystemService());
            serviceGroup.Add(new InitAdMobSystemService());

            return serviceGroup;
        }

        private Service CreateBootstrapLoadingServiceGroup()
        {
            var serviceGroup = new BootstrapLoadingServiceGroup("Bootstrap Loading Service Group");

            serviceGroup.Add(new InitSaveGameDataSystemService());
            serviceGroup.Add(new InitFTUESystemService());
            serviceGroup.Add(new InitOpenCharacterSystem());
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