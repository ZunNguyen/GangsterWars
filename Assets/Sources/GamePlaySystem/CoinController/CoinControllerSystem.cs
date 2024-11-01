using Cysharp.Threading.Tasks;
using Sources.GameData;
using Sources.SystemService;
using Sources.Utils.Singleton;
using System;
using UniRx;
using UnityEngine;

namespace Sources.GamePlaySystem.CoinController
{
    public class InitCoinControllerSystemService : InitSystemService<CoinControllerSystem> { }

    public class CoinRewardInfo
    {
        public Transform PosSpawn;
        public int Coins;
    }

    public class CoinControllerSystem : BaseSystem
    {
        private GameData.GameData _gameData => Locator<GameData.GameData>.Instance;
        private UserProfile _userProfile => _gameData.GetProfileData<UserProfile>();

        public ReactiveProperty<int> Coins { get; private set; } = new();

        public Action<CoinRewardInfo> CoinReward;

        public override async UniTask Init()
        {
            Coins.Value = _userProfile.Coins;
        }

        public void AddCoin(int quantity)
        {
            Coins.Value += quantity;
            SaveCoin();
        }

        public void SubstractCoin(int quantity)
        {
            Coins.Value -= quantity;
            SaveCoin();
        }

        private void SaveCoin()
        {
            _userProfile.Coins = Coins.Value;
            _userProfile.Save();
        }

        public void SpawnCoinReward(CoinRewardInfo coinRewardInfo)
        {
            if (coinRewardInfo.Coins == 0) return;

            CoinReward?.Invoke(coinRewardInfo);
        }

        public bool PurchaseItem(int itemCoin)
        {
            if (itemCoin <= Coins.Value)
            {
                Coins.Value -= itemCoin;
                _userProfile.Coins = Coins.Value;
                _userProfile.Save();
                return true;
            }

            else return false;
        }
    }
}

