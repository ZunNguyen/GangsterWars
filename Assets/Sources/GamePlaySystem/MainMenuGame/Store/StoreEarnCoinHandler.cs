using Cysharp.Threading.Tasks;
using Sources.AdMob;
using Sources.Audio;
using Sources.DataBaseSystem;
using Sources.Extension;
using Sources.GameData;
using Sources.TimeManager;
using Sources.Utils.Singleton;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniRx;

namespace Sources.GamePlaySystem.MainMenuGame.Store
{
    public class StoreEarnCoinHandler
    {
        private GameData.GameData _gameData => Locator<GameData.GameData>.Instance;
        private PackEarnCoinProfile _packEarnCoinProfile => _gameData.GetProfileData<PackEarnCoinProfile>();

        private DataBase _dataBase => Locator<DataBase>.Instance;
        private EarnCoinConfig _earnCoinConfig => _dataBase.GetConfig<EarnCoinConfig>();

        private AdMobSystem _adMobSystem => Locator<AdMobSystem>.Instance;
        private TimeManagerSystem _timeManagerSystem => Locator<TimeManagerSystem>.Instance;
        private AudioManager _audioManager => Locator<AudioManager>.Instance;

        private Dictionary<string, PackEarnCoinViewHandler> _packEarnCoinViewHandlers = new();
        private Dictionary<string, bool> _listCanEarnCoins = new();

        public ReactiveProperty<bool> IsAnyPackToEarn = new(false);

        public async UniTask OnSetUp()
        {
            await UniTask.WaitUntil(() => _timeManagerSystem.IsCompleteSetTimeLogin == true);

            SetPackEarnCoinViewModels();
        }

        private async void SetPackEarnCoinViewModels()
        {
            var packEarnCoinDatas = _packEarnCoinProfile.PackEarnCoinDatas;

            foreach (var data in packEarnCoinDatas)
            {
                PackEarnCoinViewHandler model = new ();
                await model.OnSetUp(data.Id);
                _packEarnCoinViewHandlers.Add(data.Id, model);
            }
        }

        public async Task<bool> ShowAdCoin()
        {
#if UNITY_ANDROID || UNITY_IOS || UNITY_EDITOR
            _audioManager.PauseAudio(AudioKey.MENU_SONG);

            var result = await _adMobSystem.ShowAdReward();

            _audioManager.Play(AudioKey.MENU_SONG);
            return result;
#endif
            return true;
        }

        public PackEarnCoinViewHandler GetPackEarnCoinViewHandler(string id)
        {
            return _packEarnCoinViewHandlers[id];
        }

        public void SetCanEarnCoin(string id, bool isCan)
        {
            _listCanEarnCoins[id] = isCan;
            CheckHaveAnyPackCanEarn();
        }

        private void CheckHaveAnyPackCanEarn()
        {
            if (_listCanEarnCoins.Values.Any(value => value)) IsAnyPackToEarn.Value = true;
            else IsAnyPackToEarn.Value = false;
        }
    }
}