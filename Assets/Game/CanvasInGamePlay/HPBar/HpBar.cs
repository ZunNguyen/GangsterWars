using Cysharp.Threading.Tasks;
using Game.CanvasInGamePlay.Controller;
using Sources.GamePlaySystem.MainGamePlay.Enemies;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using Unity.VisualScripting;
using Sources.SpawnerSystem;
using Sources.Utils.Singleton;

namespace Game.CanvasInGamePlay.HPBar
{
    public class HpBar : MonoBehaviour
    {
        private SpawnerManager _spawnerManager => Locator<SpawnerManager>.Instance;
        private Transform _worldTransformObject;
        private Canvas _canvas;

        [SerializeField] private RectTransform _rectTransformObject;
        [SerializeField] private Slider _slider;

        public void OnSetUp(HpBarModel hpBarModel)
        {
            _canvas = hpBarModel.Canvas;
            _worldTransformObject = hpBarModel.TransformObject;
            SetUpSlider(hpBarModel.EnemyHandler);
        }

        private void SetUpSlider(EnemyHandler enemyHandler)
        {
            _slider.maxValue = enemyHandler.HpMax;

            enemyHandler.HpCurrent.Subscribe(value =>
            {
                _slider.value = value;
                if (value <= 0)
                {
                    try
                    {
                        _spawnerManager.Release<HpBar>(this);
                    }
                    catch { }
                }

            }).AddTo(this);
        }

        private void FixedUpdate()
        {
            if (_worldTransformObject == null || _canvas == null) return;

            Vector3 screenPos = RectTransformUtility.WorldToScreenPoint(Camera.main, _worldTransformObject.position);

            Vector2 anchoredPos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvas.transform as RectTransform, screenPos, _canvas.worldCamera, out anchoredPos);

            _rectTransformObject.anchoredPosition = anchoredPos;
        }
    }
}