using Cysharp.Threading.Tasks;
using Sources.DataBaseSystem;
using Sources.Extension;
using Sources.GameData;
using Sources.Services;
using Sources.SystemService;
using Sources.Utils.Singleton;
using Sources.Utils.String;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sources.GamePlaySystem.JourneyMap
{
    public class InitJourneyMapSystemService : InitSystemService<JourneyMapSystem>{ };

    public enum DataState
    {
        Empty,
        JourneyItem,
        HorizontalItem,
        VerticalItem,
    }

    public class JourneyMapSystem : BaseSystem
    {
        private const string _dataDefault = "";

        private GameData.GameData _gameData => Locator<GameData.GameData>.Instance;
        private JourneyProfile _journeyProfile => _gameData.GetProfileData<JourneyProfile>();

        private DataBase _dataBase => Locator<DataBase>.Instance;
        private JourneyMapConfig _journeyMapConfig => _dataBase.GetConfig<JourneyMapConfig>();
        private EnemySpawnConfig _enemySpawnConfig => _dataBase.GetConfig<EnemySpawnConfig>();

        public JourneyMapData JourneyMapDataCurrent { get; private set; }
        public int IndexWaveCurrent {  get; private set; }

        public override async UniTask Init()
        {
            CheckJourneyData();
            GetMatrixMapCurrent();
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

            JourneyMapDataCurrent = _journeyMapConfig.JourneyMapDatas[indexGridMapCurrent];
        }


        public DataState GetDataState(string data_1)
        {
            if (data_1 == _dataDefault) return DataState.Empty;
            
            var baseDataJourneyItem = StringUtils.GetBaseName(data_1);
            if (baseDataJourneyItem == JourneyKey.BASE_JOURNEY_ITEM) return DataState.JourneyItem;

            var baseHorizontalItem = StringUtils.GetBaseName(data_1);
            if (baseHorizontalItem == JourneyKey.BASE_LINK_HORIZOTAL_ITEM) return DataState.HorizontalItem;

            var baseVericalItem = StringUtils.GetBaseName(data_1);
            if (baseVericalItem == JourneyKey.BASE_LINK_VERTICAL_ITEM) return DataState.VerticalItem;

            else return DataState.Empty;
        }
    }
}