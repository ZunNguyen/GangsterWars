using Sources.AdMob;
using Sources.DataBaseSystem;
using Sources.GameData;
using Sources.GamePlaySystem.CoinController;
using Sources.TimeManager;
using Sources.Utils.DateTime;
using Sources.Utils.Singleton;
using System.Collections.Generic;
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

        private Dictionary<string, PackEarnCoinViewHandler> _packEarnCoinViewHandlers = new();

        public void OnSetUp()
        {
            SetPackEarnCoinViewModels();
        }

        private void SetPackEarnCoinViewModels()
        {
            var packEarnCoinDatas = _packEarnCoinProfile.PackEarnCoinDatas;

            foreach (var data in packEarnCoinDatas)
            {
                PackEarnCoinViewHandler model = new ();
                model.OnSetUp(data.Id);
                _packEarnCoinViewHandlers.Add(data.Id, model);
            }
        }

        public async Task<bool> ShowAdCoin()
        {
            var result = await _adMobSystem.ShowAdReward();
            return result;
        }

        public PackEarnCoinViewHandler GetPackEarnCoinViewHandler(string id)
        {
            return _packEarnCoinViewHandlers[id];
        }
    }
}