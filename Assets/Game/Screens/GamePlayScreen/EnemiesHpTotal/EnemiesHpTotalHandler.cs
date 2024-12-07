using Sources.GamePlaySystem.MainGamePlay;
using Sources.Utils.Singleton;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using Cysharp.Threading.Tasks;
using Unity.VisualScripting;
using DG.Tweening;

namespace Game.Screens.GamePlayScreen
{
    public class EnemiesHpTotalHandler : HpHandlerAbstract
    {
        public override async void OnSetUp()
        {
            await UniTask.Delay(1000);

            _maxValue = _mainGamePlaySystem.EnemiesController.TotalHpEnemies;

            _mainGamePlaySystem.EnemiesController.HpEnemiesCurrent.Subscribe(value =>
            {
                ChangeValue(value);
            }).AddTo(this);
        }
    }
}