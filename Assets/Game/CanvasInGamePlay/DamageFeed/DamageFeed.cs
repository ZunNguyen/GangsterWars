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
        private readonly Vector3 _targetScale = new Vector3(1.2f, 1.2f, 1.2f);
        private const float _offsetMovePosY = 30f;
        private const float _duration = 0.8f;

        private SpawnerManager _spawnerManager => Locator<SpawnerManager>.Instance;

        private Tween _sequence;

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

        private async void ShowDamageFeed()
        {
            var targetMovePosY = _rectTransformObject.anchoredPosition.y + _offsetMovePosY;

            var _sequence = DOTween.Sequence();
            _sequence.Append(_rectTransformObject.DOAnchorPosY(targetMovePosY, _duration));
            _sequence.Join(_rectTransformObject.DOScale(_targetScale, _duration/2));
            _sequence.Append(_rectTransformObject.DOScale(Vector3.one, _duration / 4));
            await _sequence.Play();

            _spawnerManager.Release(this);
        }

        private void OnDestroy()
        {
            _sequence?.Kill();
        }
    }
}