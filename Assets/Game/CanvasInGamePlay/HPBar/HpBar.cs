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

        private Vector3 _screenPos;
        private Vector2 _anchoredPos;

        [SerializeField] private RectTransform _rectTransformObject;
        [SerializeField] private GameObject _hpBar;

        public override void OnSetUp(){}

        public void OnSetUp(CanvasModel canvasModel)
        {
            _canvas = canvasModel.Canvas;
            _worldTransformObject = canvasModel.TransformObject;
            gameObject.SetActive(true);
            _hpBar.SetActive(false);

            SetPos();
            SetUpSlider(canvasModel.EnemyHandler);
        }

        private void SetUpSlider(EnemyHandler enemyHandler)
        {
            _maxValue = enemyHandler.HpMax;

            _disposedHpBar = enemyHandler.HpCurrent.Subscribe(value =>
            {
                var isShowSlider = value != enemyHandler.HpMax && value > 0;
                _hpBar.SetActive(isShowSlider);
                
                if (value > 0) ChangeValue(value);

                if (value <= 0)
                {
                    try
                    {
                        _worldTransformObject = null;
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
            _screenPos = RectTransformUtility.WorldToScreenPoint(Camera.main, _worldTransformObject.position);

            RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvas.transform as RectTransform, _screenPos, _canvas.worldCamera, out _anchoredPos);

            _rectTransformObject.anchoredPosition = _anchoredPos;
        }
    }
}