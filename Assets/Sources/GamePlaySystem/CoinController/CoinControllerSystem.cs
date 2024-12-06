using Cysharp.Threading.Tasks;
using Sources.Audio;
using Sources.Extension;
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

        private AudioManager _audioManager => Locator<AudioManager>.Instance;

        public ReactiveProperty<int> Coins { get; private set; } = new();

        public Action<CoinRewardInfo> CoinReward;
        public Action<int> OnAddCoin;
        public Action<int> OnSubstractCoin;

        public override async UniTask Init()
        {
            Coins.Value = _userProfile.Coins;
        }

        public void AddCoin(int quantity)
        {
            _audioManager.Play(AudioKey.SFX_EARN_COIN);

            Coins.Value += quantity;
            OnAddCoin?.Invoke(quantity);
            SaveCoin();
        }

        public void SubstractCoin(int quantity)
        {
            Coins.Value -= quantity;
            OnSubstractCoin?.Invoke(quantity);
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

        public bool PurchaseItem(int fee)
        {
            if (fee <= Coins.Value)
            {
                SubstractCoin(fee);
                return true;
            }

            else return false;
        }
    }
}

