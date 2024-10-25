using Cysharp.Threading.Tasks;
using Sources.GamePlaySystem.CoinController;
using Sources.Utils;
using Sources.Utils.Singleton;
using UnityEngine;
using DG.Tweening;
using Sources.SpawnerSystem;

namespace Game.Screens.Coin
{
    public class Coin : MonoBehaviour
    {
        private const int _speed = 30;
     
        private SpawnerManager _spawnerManager => Locator<SpawnerManager>.Instance;
        private CoinControllerSystem _coinControllerSystem => Locator<CoinControllerSystem>.Instance;

        public async void OnSetUp(Transform posCoinsHolder, int quantity)
        {
            await UniTask.Delay(5000);
            var duration = TweenUtils.GetTimeDuration(transform.position, posCoinsHolder.position, _speed);
            transform.DOMove(posCoinsHolder.position, duration).OnComplete(() =>
            {
                _coinControllerSystem.AddCoin(quantity);
                _spawnerManager.Release(this);
            });
        }
    }
}