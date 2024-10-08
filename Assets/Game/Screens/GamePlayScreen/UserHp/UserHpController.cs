using Cysharp.Threading.Tasks;
using Sources.GamePlaySystem.MainGamePlay;
using Sources.Utils.Singleton;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

namespace Game.Screens.GamePlayScreen
{
    public class UserHpController : MonoBehaviour
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