using Cysharp.Threading.Tasks;
using Game.CanvasInGamePlay.Controller;
using Sources.GamePlaySystem.MainGamePlay.Enemies;
using Sources.SpawnerSystem;
using Sources.Utils.Singleton;
using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Game.CanvasInGamePlay.HPBar
{
    public class HpBar : MonoBehaviour
    {
        private SpawnerManager _spawnerManager => Locator<SpawnerManager>.Instance;
        private Transform _worldTransformObject;
        private Canvas _canvas;
        private IDisposable _disposedHpBar;

        [SerializeField] private RectTransform _rectTransformObject;
        [SerializeField] private Slider _slider;

        public void OnSetUp(CanvasModel canvasModel)
        {
            _canvas = canvasModel.Canvas;
            _worldTransformObject = canvasModel.TransformObject;
            SetUpSlider(canvasModel.EnemyHandler);
        }

        private void SetUpSlider(EnemyHandler enemyHandler)
        {
            _slider.maxValue = enemyHandler.HpMax;

            _disposedHpBar = enemyHandler.HpCurrent.Subscribe(value =>
            {
                _slider.value = value;
                if (value <= 0)
                {
                    try
                    {
                        _disposedHpBar?.Dispose();
                        _spawnerManager.Release(gameObject);
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