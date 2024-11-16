using Game.CanvasInGamePlay.Controller;
using Sources.GamePlaySystem.MainGamePlay.Enemies;
using Sources.SpawnerSystem;
using Sources.Utils.Singleton;
using System;
using TMPro;
using UniRx;
using UnityEngine;
using DG.Tweening;
using Cysharp.Threading.Tasks;

namespace Game.CanvasInGamePlay
{
    public class DamageFeed : MonoBehaviour
    {
        private const float _offsetMovePosY = 30f;
        private const float _duration = 1f;

        private SpawnerManager _spawnerManager => Locator<SpawnerManager>.Instance;

        private bool _isShowText = false;

        private Transform _worldTransformObject;
        private Canvas _canvas;
        private EnemyHandler _enemyHandler;
        private IDisposable _disposedHpBar;

        [SerializeField] private RectTransform _rectTransformObject;
        [SerializeField] private TMP_Text _text;

        public void OnSetUp(CanvasModel canvasModel)
        {
            gameObject.SetActive(false);

            _canvas = canvasModel.Canvas;
            _worldTransformObject = canvasModel.TransformObject;
            _enemyHandler = canvasModel.EnemyHandler;

            _disposedHpBar = _enemyHandler.HpCurrent.Subscribe(async value =>
            {
                if (value <= 0)
                {
                    while(_isShowText)
                    {
                        await UniTask.DelayFrame(1);
                    }

                    try
                    {
                        _disposedHpBar?.Dispose();
                        _enemyHandler.DamageFeed -= ShowDamageFeed;
                        _spawnerManager.Release(this);
                    }
                    catch { }
                }

            }).AddTo(this);

            _enemyHandler.DamageFeed += ShowDamageFeed;
        }

        private void ShowDamageFeed(int value)
        {
            _isShowText = true;

            gameObject.SetActive(true);
            _text.text = value.ToString();

            Vector3 screenPos = RectTransformUtility.WorldToScreenPoint(Camera.main, _worldTransformObject.position);
            Vector2 anchoredPos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvas.transform as RectTransform, screenPos, _canvas.worldCamera, out anchoredPos);
            _rectTransformObject.anchoredPosition = anchoredPos;

            var targetMovePosY = _rectTransformObject.anchoredPosition.y + _offsetMovePosY;
            _rectTransformObject.DOAnchorPosY(targetMovePosY, _duration).OnComplete(async () =>
            {
                await UniTask.Delay(200);
                gameObject.SetActive(false);

                _isShowText = false;
            });
        }
    }
}