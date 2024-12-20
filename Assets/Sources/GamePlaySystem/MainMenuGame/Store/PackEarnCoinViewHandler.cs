using Sources.DataBaseSystem;
using Sources.GameData;
using Sources.GamePlaySystem.CoinController;
using Sources.TimeManager;
using Sources.Utils.Singleton;
using UniRx;
using UnityEditor;

namespace Sources.GamePlaySystem.MainMenuGame.Store
{
    public class PackEarnCoinViewHandler
    {
        private GameData.GameData _gameData => Locator<GameData.GameData>.Instance;
        private PackEarnCoinProfile _packEarnCoinProfile => _gameData.GetProfileData<PackEarnCoinProfile>();

        private DataBase _dataBase => Locator<DataBase>.Instance;
        private EarnCoinConfig _earnCoinConfig => _dataBase.GetConfig<EarnCoinConfig>();

        private CoinControllerSystem _coinControllerSystem => Locator<CoinControllerSystem>.Instance;
        private TimeManagerSystem _timeManagerSystem => Locator<TimeManagerSystem>.Instance;

        private string _selfId;
        private int _coinValue;

        public int TimeToEarn { get; private set; }
        public int CountCoinIcon { get; private set; }
        public ReactiveProperty<int> TimeRemain { get; private set; } = new();

        public void OnSetUp(string id)
        {
            _selfId = id;

            SetValue();
            SubscribeAddOneSecond();

#if UNITY_EDITOR
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
#endif
        }

        private void SetValue()
        {
            var packEarnCoinData = _packEarnCoinProfile.GetPackEarnCoinData(_selfId);
            TimeRemain.Value = packEarnCoinData.TimeNextEarn;

            var packEarnCoinInfo = _earnCoinConfig.GetEarnCoinInfo(_selfId);
            TimeToEarn = packEarnCoinInfo.TimeToReload;

            var buyCoinInfo = _earnCoinConfig.GetEarnCoinInfo(_selfId);
            _coinValue = buyCoinInfo.Value;

            CountCoinIcon = _earnCoinConfig.CountCoinIcon;
        }

        private void CheckCanEarn()
        {
            if (TimeRemain.Value == 0)
            {
                UnSubscribeAddOneSecond();
                return;
            }

            TimeRemain.Value--;
        }

        private void SubscribeAddOneSecond()
        {
            _timeManagerSystem.AddOneSecondTimeOnline += CheckCanEarn;
        }

        private void UnSubscribeAddOneSecond()
        {
            _timeManagerSystem.AddOneSecondTimeOnline -= CheckCanEarn;
        }

        public void ResetTimeToEarn()
        {
            _packEarnCoinProfile.UpdateTimeNextEarn(_selfId, TimeToEarn);
            TimeRemain.Value = TimeToEarn;
            SubscribeAddOneSecond();
        }

        public void AddCoin()
        {
            _coinControllerSystem.AddCoin(_coinValue / CountCoinIcon);
        }

        private void OnApplicationQuit()
        {
            _packEarnCoinProfile.UpdateTimeNextEarn(_selfId, TimeRemain.Value);
        }

#if UNITY_EDITOR
        private void OnPlayModeStateChanged(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.ExitingPlayMode)
            {
                _packEarnCoinProfile.UpdateTimeNextEarn(_selfId, TimeRemain.Value);
            }
        }
#endif
    }
}