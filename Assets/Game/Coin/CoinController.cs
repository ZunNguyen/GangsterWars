using Sources.GamePlaySystem.CoinController;
using Sources.SpawnerSystem;
using Sources.Utils.Singleton;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Screens.Coin
{
    public class CoinController : MonoBehaviour
    {
        private SpawnerManager _spawnerManager => Locator<SpawnerManager>.Instance;
        private CoinControllerSystem _coinControllerSystem => Locator<CoinControllerSystem>.Instance;

        [SerializeField] private Coin _coinPrefab;
        [SerializeField] private Transform _posIconCoinReward;

        private void Awake()
        {
            _coinControllerSystem.CoinReward += SpawnCoinReward;
            _coinPrefab.gameObject.SetActive(false);
        }

        private void SpawnCoinReward(CoinRewardInfo coinRewardInfo)
        {
            var newCoin = _spawnerManager.Get(_coinPrefab);
            newCoin.transform.position = coinRewardInfo.PosSpawn.position;
            newCoin.gameObject.SetActive(true);
            newCoin.OnSetUp(_posIconCoinReward, coinRewardInfo.Coins);
        }
    }
}