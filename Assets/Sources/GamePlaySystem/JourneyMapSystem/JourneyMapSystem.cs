using Cysharp.Threading.Tasks;
using Sources.Command;
using Sources.DataBaseSystem;
using Sources.Extension;
using Sources.GameData;
using Sources.GamePlaySystem.MainGamePlay;
using Sources.SystemService;
using Sources.Utils.Singleton;
using Sources.Utils.String;
using System.Collections.Generic;

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

        private List<JourneyMapData> _journeyMapDatas = new();

        public int StarCurrent { get; private set; }
        public int IndexGridMapCurrent { get; private set; }
        public int IndexGridMapMaxCurrent { get; private set; }
        public JourneyMapData JourneyMapDataCurrent { get; private set; }

        public override async UniTask Init()
        {
            GetAllMatrixMapAvailable();
            ChangeJourneyMap(IndexGridMapCurrent);
        }

        public void ChangeJourneyMap(int index)
        {
            StarCurrent = 0;
            JourneyMapDataCurrent = _journeyMapDatas[index];
            IndexGridMapCurrent = index;
        }

        private void GetAllMatrixMapAvailable()
        {
            var countMaxWaveCurrent = _journeyProfile.WavesPassedDatas.Count;

            var journeyItemMaxInOneGrid = _journeyMapConfig.JourneyItemViews.Count;
            IndexGridMapCurrent = IndexGridMapMaxCurrent = countMaxWaveCurrent / journeyItemMaxInOneGrid;

            for (int i = 0; i <= IndexGridMapCurrent; i++)
            {
                var journeyMapData = _journeyMapConfig.JourneyMapDatas[i];
                _journeyMapDatas.Add(journeyMapData);
            }
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
            CountStarInEpisode(waveId);

            if (_journeyProfile.HaveWaveData(waveId)) return JourneyItemState.Passed;

            if (IsWaveFirstInEpisode(waveId)) return JourneyItemState.NotYetPass;

            if (CheckJourneyItemHorizontalPreviousPassed(waveId)) return JourneyItemState.NotYetPass;

            if (CheckJourneyItemVerticalPreviousPassed(waveId)) return JourneyItemState.NotYetPass;
            
            return JourneyItemState.Lock;
        }

        private bool IsWaveFirstInEpisode(string waveId)
        {
            return waveId == WaveKey.WAVE_ID_FIRST_EPISODE_1
                || waveId == WaveKey.WAVE_ID_FIRST_EPISODE_2
                || waveId == WaveKey.WAVE_ID_FIRST_EPISODE_3
                || waveId == WaveKey.WAVE_ID_FIRST_EPISODE_4;
        }

        private bool CheckJourneyItemHorizontalPreviousPassed(string waveId)
        {
            var indexCurrent = _journeyMapConfig.GetIndexWaveIdInJourneyMap(IndexGridMapCurrent, waveId);
            
            var indexLinkItem = --indexCurrent;
            var haveLinkId = _journeyMapConfig.HaveLinkIdWithIndex(IndexGridMapCurrent, indexLinkItem);
            if (!haveLinkId) return false;

            var indexWaveIdPrevious = --indexCurrent;
            var waveIdPrevious = _journeyMapConfig.GetWaveIdWithIndex(IndexGridMapCurrent, indexWaveIdPrevious);
            return _journeyProfile.HaveWaveData(waveIdPrevious);
        }

        private bool CheckJourneyItemVerticalPreviousPassed(string waveId)
        {
            var indexCurrent = _journeyMapConfig.GetIndexWaveIdInJourneyMap(IndexGridMapCurrent, waveId);
            var offsetCol = _journeyMapConfig.JourneyMapDatas[IndexGridMapCurrent].Collumns;

            var indexLinkItemSubstract = indexCurrent - offsetCol;
            if (_journeyMapConfig.HaveLinkIdWithIndex(IndexGridMapCurrent, indexLinkItemSubstract))
            {
                var indexWaveIdPrevious = indexLinkItemSubstract - offsetCol;
                var waveIdPrevious = _journeyMapConfig.GetWaveIdWithIndex(IndexGridMapCurrent, indexWaveIdPrevious);
                if (_journeyProfile.HaveWaveData(waveIdPrevious)) return true;
            }

            var indexLinkItemAdd = indexCurrent + offsetCol;
            if (!_journeyMapConfig.HaveLinkIdWithIndex(IndexGridMapCurrent, indexLinkItemAdd)) return false;
            else
            {
                var indexWaveIdPrevious = indexLinkItemAdd + offsetCol;
                var waveIdPrevious = _journeyMapConfig.GetWaveIdWithIndex(IndexGridMapCurrent, indexWaveIdPrevious);
                return _journeyProfile.HaveWaveData(waveIdPrevious);
            }
        }

        private void CountStarInEpisode(string waveId)
        {
            var waveData = _journeyProfile.GetWaveData(waveId);
            if (waveData == null) return;
            StarCurrent += waveData.Stars;
        }

        public async void OnBattleWave(string waveId)
        {
            new LoadGamePlayScenceCommand(waveId).Execute().Forget();
        }

        public async void OnChangeEpisode(int index)
        {
            if (index == IndexGridMapCurrent) return;
            await new LoadJourneyMapCommand(index).Execute();
        }
    }
}