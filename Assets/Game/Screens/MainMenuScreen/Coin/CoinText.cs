using DG.Tweening;
using Sources.SpawnerSystem;
using Sources.Utils.Singleton;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Game.Screens.MainMenuScreen
{
    public class CoinText : MonoBehaviour
    {
        private const float _offsetMoveY = 80f;
        private const float _duration = 0.4f;

        private const string _stringAdd = "+";
        private const string _stringSubstract = "-";

        private SpawnerManager _spawnerManager => Locator<SpawnerManager>.Instance;

        private float _targetMoveY;

        [SerializeField] private RectTransform _rect;
        [SerializeField] private TMP_Text _text;

        public void OnSetUp(int quality, bool isAdd)
        {
            _text.text = (isAdd ? _stringAdd : _stringSubstract) + quality.ToString();

            _targetMoveY = _rect.anchoredPosition.y + _offsetMoveY;
            _text.alpha = 1f;
            AnimationMove();
        }

        private void AnimationMove()
        {
            var sequence = DOTween.Sequence();

            sequence.Append(_rect.DOAnchorPosY(_targetMoveY, _duration).SetEase(Ease.OutQuart));
            sequence.Insert(_duration*0.75f, _text.DOFade(0, _duration));
            sequence.AppendCallback( () => _spawnerManager.Release(gameObject));

            sequence.Play();
        }
    }
}