using Cysharp.Threading.Tasks;
using Sources.GamePlaySystem.MainGamePlay;
using Sources.Utils.Singleton;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Screens.GamePlayScreen
{
    public class UserHpHandler : MonoBehaviour
    {
        private MainGamePlaySystem _mainGamePlaySystem => Locator<MainGamePlaySystem>.Instance;

        [SerializeField] private Slider _slider;

        public void OnSetUp()
        {
            _slider.maxValue = _mainGamePlaySystem.UserRecieveDamageHandler.HpCurrentUser.Value;

            _mainGamePlaySystem.UserRecieveDamageHandler.HpCurrentUser.Subscribe(value =>
            {
                _slider.value = value;
            }).AddTo(this);
        }
    }
}