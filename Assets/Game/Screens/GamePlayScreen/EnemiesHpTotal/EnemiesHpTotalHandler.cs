using Sources.GamePlaySystem.MainGamePlay;
using Sources.Utils.Singleton;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using Cysharp.Threading.Tasks;

namespace Game.Screens.GamePlayScreen
{
    public class EnemiesHpTotalHandler : MonoBehaviour
    {
        private MainGamePlaySystem _mainGamePlaySystem => Locator<MainGamePlaySystem>.Instance;

        [SerializeField] private Slider _slider;

        public async void OnSetUp()
        {
            await UniTask.Delay(1000);
            _slider.maxValue = _slider.value = _mainGamePlaySystem.EnemiesController.TotalHpEnemies;

            _mainGamePlaySystem.EnemiesController.HpEnemiesCurrent.Subscribe(value =>
            {
                _slider.value = value;
            }).AddTo(this);
        }
    }
}