using Sources.DataBaseSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Sources.UI;
using DG.Tweening;
using Cysharp.Threading.Tasks;

namespace Game.Screens.BlackLoadingScreen
{
    public class BlackLoadingScreen : BaseUI
    {
        private readonly Vector2 _anchoMinTopTarget = new Vector2(0, 1);
        private readonly Vector2 _anchoMinBotTarget = new Vector2(1, 0);
        private const float _duration = 0.5f;

        private Vector2 _originalAnchorPanelTop;
        private Vector2 _originalAnchorPanelBot;

        [SerializeField] private RectTransform _panelTop;
        [SerializeField] private RectTransform _panelBot;

        protected override void Awake()
        {
            base.Awake();

            _originalAnchorPanelTop = _panelTop.anchorMin;
            _originalAnchorPanelBot = _panelBot.anchorMax;

            _panelTop.anchorMin = _anchoMinTopTarget;
            _panelBot.anchorMax = _anchoMinBotTarget;
        }

        public async UniTask PanelMoveIn()
        {
            _panelTop.DOAnchorMin(_originalAnchorPanelTop, _duration).SetEase(Ease.OutQuart);
            await _panelBot.DOAnchorMax(_originalAnchorPanelBot, _duration).SetEase(Ease.OutQuart);
        }

        public async UniTask PanelMoveOut()
        {
            _panelTop.DOAnchorMin(_anchoMinTopTarget, _duration).SetEase(Ease.OutQuart);
            await _panelBot.DOAnchorMax(_anchoMinBotTarget, _duration).SetEase(Ease.OutQuart);
        }
    }
}