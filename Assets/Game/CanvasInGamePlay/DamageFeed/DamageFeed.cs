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

        private Tween _tween;

        [SerializeField] private RectTransform _rectTransformObject;
        [SerializeField] private TMP_Text _text;

        public void OnSetUp(Canvas canvas, Transform transformObject, int damageFeed)
        {
            _text.text = damageFeed.ToString();
            
            Vector3 screenPos = RectTransformUtility.WorldToScreenPoint(Camera.main, transformObject.position);
            Vector2 anchoredPos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, screenPos, canvas.worldCamera, out anchoredPos);
            _rectTransformObject.anchoredPosition = anchoredPos;

            ShowDamageFeed();
        }

        private void ShowDamageFeed()
        {
            var targetMovePosY = _rectTransformObject.anchoredPosition.y + _offsetMovePosY;
            _tween = _rectTransformObject.DOAnchorPosY(targetMovePosY, _duration).OnComplete(async () =>
            {
                await UniTask.Delay(200);
                _spawnerManager.Release(this);
            });
        }

        private void OnDestroy()
        {
            _tween?.Kill();
        }
    }
}