using Cysharp.Threading.Tasks;
using DG.Tweening;
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
        private const float _duration = 0.5f;

        private SpawnerManager _spawnerManager => Locator<SpawnerManager>.Instance;
        private Transform _worldTransformObject;
        private Canvas _canvas;
        private IDisposable _disposedHpBar;

        private bool _isActive;

        [SerializeField] private RectTransform _rectTransformObject;
        [SerializeField] private Slider _slider;
        [SerializeField] private Image _fillWhite;

        private void Awake()
        {
            _slider.onValueChanged.AddListener(OnSliderValueChange);
        }

        public async void OnSetUp(CanvasModel canvasModel)
        {
            _isActive = true;



            
            _canvas = canvasModel.Canvas;
            _worldTransformObject = canvasModel.TransformObject;
            SetUpSlider(canvasModel.EnemyHandler);

            await UniTask.DelayFrame(2);
            gameObject.SetActive(true);
        }

        private void SetUpSlider(EnemyHandler enemyHandler)
        {
            _slider.maxValue = enemyHandler.HpMax;

            _disposedHpBar = enemyHandler.HpCurrent.Subscribe(value =>
            {
                _slider.value = value;
                if (value <= 0)
                {
                    _isActive = false;
                    try
                    {
                        _disposedHpBar?.Dispose();
                        _spawnerManager.Release(gameObject);
                    }
                    catch { }
                }

            }).AddTo(this);
        }

        private async void OnSliderValueChange(float value)
        {
            await UniTask.Delay(1000);

            DOTween.To(() =>
                    _fillWhite.fillAmount,
                    x => _fillWhite.fillAmount = x,
                    value / _slider.maxValue,
                    _duration
                ).SetEase(Ease.OutQuart);
        }

        private void OnDestroy()
        {
            _slider.onValueChanged.RemoveListener(OnSliderValueChange);
        }

        private void FixedUpdate()
        {
            if (!_isActive) return;

            if (_worldTransformObject == null || _canvas == null) return;

            Vector3 screenPos = RectTransformUtility.WorldToScreenPoint(Camera.main, _worldTransformObject.position);

            Vector2 anchoredPos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvas.transform as RectTransform, screenPos, _canvas.worldCamera, out anchoredPos);

            _rectTransformObject.anchoredPosition = anchoredPos;
        }
    }
}