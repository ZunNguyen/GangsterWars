using Sources.AdMob;
using Sources.DataBaseSystem;
using Sources.GamePlaySystem.CoinController;
using Sources.Utils.Singleton;
using System.Threading.Tasks;

namespace Sources.GamePlaySystem.MainMenuGame.Store
{
    public class StoreBuyCoinHandler
    {
        private DataBase _dataBase => Locator<DataBase>.Instance;
        private BuyCoinConfig _buyCoinConfig => _dataBase.GetConfig<BuyCoinConfig>();

        private CoinControllerSystem _coinControllerSystem => Locator<CoinControllerSystem>.Instance;
        private AdMobSystem _adMobSystem => Locator<AdMobSystem>.Instance;

        private int _coinValue;
        
        public int _countCoinIcon { get; private set; }

        public void OnSetUp()
        {
            _countCoinIcon = _buyCoinConfig.CountCoinIcon;
        }

        public async Task<bool> ShowAdCoin(string id)
        {
            var result = await _adMobSystem.ShowAdReward();
         
            if (result)
            {
                var buyCoinInfo = _buyCoinConfig.GetBuyCoinInfo(id);
                _coinValue = buyCoinInfo.Value;
            }

            return result;
        }

        public void AddCoin()
        {
            _coinControllerSystem.AddCoin(_coinValue / _countCoinIcon);
        }
    }
}