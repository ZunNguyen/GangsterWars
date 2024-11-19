using Cysharp.Threading.Tasks;
using Sources.Command;
using Sources.DataBaseSystem;
using Sources.Extension;
using Sources.GameData;
using Sources.GamePlaySystem.MainGamePlay;
using Sources.SystemService;
using Sources.Utils.Singleton;
using Sources.Utils.String;
using System;
using System.Threading.Tasks;

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

    public enum JourneyItemState
    {
        Passed,
        NotYetPass,
        Lock
    }

    public class JourneyMapSystem : BaseSystem
    {
        private const string _dataDefault = "";

        private GameData.GameData _gameData => Locator<GameData.GameData>.Instance;
        private JourneyProfile _journeyProfile => _gameData.GetProfileData<JourneyProfile>();

        private DataBase _dataBase => Locator<DataBase>.Instance;
        private JourneyMapConfig _journeyMapConfig => _dataBase.GetConfig<JourneyMapConfig>();
        private SpawnWaveConfig _enemySpawnConfig => _dataBase.GetConfig<SpawnWaveConfig>();
        private MainGamePlaySystem _mainGamePlaySystem => Locator<MainGamePlaySystem>.Instance;

        public int IndexGridMapCurrent { get; private set; }

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
            IndexGridMapCurrent = IndexWaveCurrent / journeyItemMaxInOneGrid;

            JourneyMapDataCurrent = _journeyMapConfig.JourneyMapDatas[IndexGridMapCurrent];
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

        public JourneyItemState GetJourneyItemState(string waveId)
        {
            if (_journeyProfile.HaveWaveData(waveId)) return JourneyItemState.Passed;

            if (CheckJourneyItemHorizontalPreviousPassed(waveId)) return JourneyItemState.NotYetPass;

            if (CheckJourneyItemVerticalPreviousPassed(waveId)) return JourneyItemState.NotYetPass;
            
            return JourneyItemState.Lock;
        }

        private bool CheckJourneyItemHorizontalPreviousPassed(string waveId)
        {
            var indexCurrent = _journeyMapConfig.GetIndexWaveIdInJourneyMap(IndexGridMapCurrent, waveId);
            
            var indexLinkItem = indexCurrent--;
            var haveLinkId = _journeyMapConfig.HaveLinkIdWithIndex(IndexGridMapCurrent, indexLinkItem);
            if (!haveLinkId) return false;

            var indexWaveIdPrevious = indexCurrent--;
            var waveIdPrevious = _journeyMapConfig.GetWaveIdWithIndex(IndexGridMapCurrent, indexWaveIdPrevious);
            return _journeyProfile.HaveWaveData(waveIdPrevious);
        }

        private bool CheckJourneyItemVerticalPreviousPassed(string waveId)
        {
            var indexCurrent = _journeyMapConfig.GetIndexWaveIdInJourneyMap(IndexGridMapCurrent, waveId);
            var offsetCol = _journeyMapConfig.JourneyMapDatas[IndexGridMapCurrent].Collumns;

            var indexLinkItemSubstract = indexCurrent - offsetCol;
            if (!_journeyMapConfig.HaveLinkIdWithIndex(IndexGridMapCurrent, indexLinkItemSubstract)) return false;
            else
            {
                var indexWaveIdPrevious = indexCurrent - offsetCol;
                var waveIdPrevious = _journeyMapConfig.GetWaveIdWithIndex(IndexGridMapCurrent, indexWaveIdPrevious);
                if (_journeyProfile.HaveWaveData(waveIdPrevious)) return true;
            }

            var indexLinkItemAdd = indexCurrent + offsetCol;
            if (!_journeyMapConfig.HaveLinkIdWithIndex(IndexGridMapCurrent, indexLinkItemAdd)) return false;
            else
            {
                var indexWaveIdPrevious = indexCurrent - offsetCol;
                var waveIdPrevious = _journeyMapConfig.GetWaveIdWithIndex(IndexGridMapCurrent, indexWaveIdPrevious);
                return _journeyProfile.HaveWaveData(waveIdPrevious);
            }
        }

        public async void OnBattleWave(string waveId)
        {
            new LoadGamePlayScenceCommand(waveId).Execute().Forget();
        }

        public void OnChangeEpisode(int index)
        {

        }
    }
}