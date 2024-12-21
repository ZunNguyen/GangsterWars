using DG.Tweening;
using Sources.GamePlaySystem.CoinController;
using Sources.SpawnerSystem;
using Sources.Utils;
using Sources.Utils.Singleton;
using UnityEngine;

namespace Game.Screens.Coin
{
    public class Coin : MonoBehaviour
    {
        private const int _speed = 30;
     
        private SpawnerManager _spawnerManager => Locator<SpawnerManager>.Instance;
        private CoinControllerSystem _coinControllerSystem => Locator<CoinControllerSystem>.Instance;

        private Tween _tween;

        public void OnSetUp(Transform posCoinsHolder, int quantity, int speed)
        {
            var duration = TweenUtils.GetTimeDuration(transform.position, posCoinsHolder.position, speed);
            _tween = transform.DOMove(posCoinsHolder.position, duration).SetEase(Ease.Linear).OnComplete(() =>
            {
                _coinControllerSystem.AddCoin(quantity);
                _spawnerManager.Release(gameObject);
            });
        }

        private void OnDestroy()
        {
            _tween.Kill();
        }
    }
}