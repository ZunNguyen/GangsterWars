using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Screens.GamePlayScreen
{
    public class PanelHandler : MonoBehaviour
    {
        private readonly Vector2 _offsetPos = new Vector2(0, 1000f);
        private const float _duration = 1f;

        private bool _isMovingPanelPause = false;
        private bool _isMovingPanelSetting = false;

        private Vector2 _originalPosPanelPause;
        private Vector2 _originalPosPanelSetting;

        [SerializeField] private RectTransform _panelPause;
        [SerializeField] private RectTransform _panelSetting;

        private void Awake()
        {
            gameObject.SetActive(false);

            _originalPosPanelPause = _panelPause.anchoredPosition;
            _originalPosPanelSetting = _panelSetting.anchoredPosition;

            _panelPause.anchoredPosition = -_offsetPos;
            _panelSetting.anchoredPosition = _offsetPos;

            gameObject.SetActive(true);
        }

        private void MovePanelPauseOn()
        {
            if (_isMovingPanelPause) return;
            _isMovingPanelPause = true;
            _panelPause.DOAnchorPos(_originalPosPanelPause, _duration).SetEase(Ease.OutQuart)
                .OnComplete(() => _isMovingPanelPause = false);
        }

        private void MovePanelPauseOut()
        {
            if (_isMovingPanelPause) return;
            _isMovingPanelPause = true;
            _panelPause.DOAnchorPos(-_offsetPos, _duration).SetEase(Ease.OutQuart)
                .OnComplete(() => _isMovingPanelPause = false);
        }

        private void MovePanelSettingOn()
        {
            if (_isMovingPanelSetting) return;
            _isMovingPanelSetting = true;
            _panelSetting.DOAnchorPos(_originalPosPanelSetting, _duration).SetEase(Ease.OutBack)
                .OnComplete(() => _isMovingPanelSetting = false);
        }

        private void MovePanelSettingOut()
        {
            if (_isMovingPanelSetting) return;
            _isMovingPanelSetting = true;
            _panelSetting.DOAnchorPos(_offsetPos, _duration).SetEase(Ease.OutBack)
                .OnComplete(() => _isMovingPanelSetting = false);
        }

        public void OnPauseClicked()
        {
            MovePanelPauseOn();
        }

        public void OnResumeClicked()
        {
            MovePanelPauseOut();
        }

        public void OnSettingClicked()
        {
            MovePanelSettingOn();
        }

        public void OnHomeClicked()
        {
            
        }

        public void OnComfirmClicked()
        {
            MovePanelSettingOut();
        }
    }
}