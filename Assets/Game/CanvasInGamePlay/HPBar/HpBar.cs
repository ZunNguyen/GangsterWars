using Cysharp.Threading.Tasks;
using Game.CanvasInGamePlay.Controller;
using Sources.GamePlaySystem.MainGamePlay;
using Sources.GamePlaySystem.MainGamePlay.Enemies;
using Sources.SpawnerSystem;
using Sources.Utils.Singleton;
using System;
using UniRx;
using UnityEngine;

namespace Game.CanvasInGamePlay.HPBar
{
    public class HpBar : HpHandlerAbstract
    {
        private SpawnerManager _spawnerManager => Locator<SpawnerManager>.Instance;
        private Transform _worldTransformObject;
        private Canvas _canvas;
        private IDisposable _disposedHpBar;

        [SerializeField] private RectTransform _rectTransformObject;

        public override void OnSetUp(){}

        public void OnSetUp(CanvasModel canvasModel)
        {
            _canvas = canvasModel.Canvas;
            _worldTransformObject = canvasModel.TransformObject;
            SetPos();
            SetUpSlider(canvasModel.EnemyHandler);
        }

        private void SetUpSlider(EnemyHandler enemyHandler)
        {
            _maxValue = enemyHandler.HpMax;

            _disposedHpBar = enemyHandler.HpCurrent.Subscribe(value =>
            {
                var isShowSlider = value != enemyHandler.HpMax && value > 0;
                gameObject.SetActive(isShowSlider);
                
                ChangeValue(value);
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
            SetPos();
        }

        private void SetPos()
        {
            Vector3 screenPos = RectTransformUtility.WorldToScreenPoint(Camera.main, _worldTransformObject.position);

            Vector2 anchoredPos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvas.transform as RectTransform, screenPos, _canvas.worldCamera, out anchoredPos);

            _rectTransformObject.anchoredPosition = anchoredPos;
        }
    }
}