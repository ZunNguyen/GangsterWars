using Cysharp.Threading.Tasks;
using DG.Tweening;
using Sources.GamePlaySystem.MainGamePlay;
using Sources.Utils.Singleton;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Screens.GamePlayScreen
{
    public class UserHpHandler : HpHandlerAbstract
    {
        public override void OnSetUp()
        {
            _maxValue = _mainGamePlaySystem.UserRecieveDamageHandler.HpCurrentUser.Value;

            _mainGamePlaySystem.UserRecieveDamageHandler.HpCurrentUser.Subscribe(value =>
            {
                ChangeValue(value);
            }).AddTo(this);
        }
    }
}