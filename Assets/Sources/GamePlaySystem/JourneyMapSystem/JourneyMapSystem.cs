using Cysharp.Threading.Tasks;
using Sources.DataBaseSystem;
using Sources.GameData;
using Sources.Services;
using Sources.SystemService;
using Sources.Utils.Singleton;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sources.GamePlaySystem.JourneyMap
{
    public class InitJourneyMapSystemService : InitSystemService<JourneyMapSystem>{ };

    public class JourneyMapSystem : BaseSystem
    {
        private GameData.GameData _gameData => Locator<GameData.GameData>.Instance;
        private JourneyProfile _journeyProfile => _gameData.GetProfileData<JourneyProfile>();

        private DataBase _dataBase => Locator<DataBase>.Instance;
        private JourneyMapConfig _journeyMapConfig => _dataBase.GetConfig<JourneyMapConfig>();
        private EnemySpawnConfig _enemySpawnConfig => _dataBase.GetConfig<EnemySpawnConfig>();

        public GridMap GridMapCurrent { get; private set; }
        public int IndexWaveCurrent {  get; private set; }

        public override async UniTask Init()
        {
            GetMatrixMapCurrent();
            CheckJourneyData();
        }

        private void CheckJourneyData()
        {
            if (_journeyProfile.WavesPassedDatas.Count == 0)
            {
                _journeyProfile.SetJourneyDataDefault();
            }
        }

        private void GetMatrixMapCurrent()
        {
            var indexMaxWaveData = _journeyProfile.WavesPassedDatas.Count - 1;
            IndexWaveCurrent = _enemySpawnConfig.GetIndexWaveInfo(_journeyProfile.WavesPassedDatas[indexMaxWaveData].Id);

            var journeyItemMaxInOneGrid = _journeyMapConfig.JourneyItemViews.Count;
            var indexGridMapCurrent = IndexWaveCurrent / journeyItemMaxInOneGrid;

            GridMapCurrent = _journeyMapConfig.GridMaps[indexGridMapCurrent];
        }
    }
}