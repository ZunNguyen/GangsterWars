using Cysharp.Threading.Tasks;
using DG.Tweening;
using Sources.Audio;
using Sources.Command;
using Sources.Extension;
using Sources.GamePlaySystem.GameResult;
using Sources.Utils.Singleton;
using UnityEngine;

namespace Game.Screens.GamePlayScreen
{
    public class PanelHandler : MonoBehaviour
    {
        private readonly Vector2 _offsetPos = new Vector2(0, 100f);
        private const float _duration = 0.7f;

        private GameResultSystem _gameResultSystem => Locator<GameResultSystem>.Instance;
        private AudioManager _audioManager => Locator<AudioManager>.Instance;

        private bool _isResumeClick = false;
        private bool _isMovingPanelPause = false;
        private bool _isMovingPanelSetting = false;

        private Vector2 _targetOffsetPos;
        private Vector2 _originalPosPanelPause;
        private Vector2 _originalPosPanelSetting;

        [SerializeField] private GameObject _blockBG;
        [SerializeField] private RectTransform _panelPause;
        [SerializeField] private RectTransform _panelSetting;
        [SerializeField] private RectTransform _mainScreen;

        public void OnSetUp()
        {
            _targetOffsetPos = _offsetPos;
            _targetOffsetPos.y += _mainScreen.rect.width/2;

            _originalPosPanelPause = _panelPause.anchoredPosition;
            _originalPosPanelSetting = _panelSetting.anchoredPosition;

            _panelPause.anchoredPosition = -_targetOffsetPos;
            _panelSetting.anchoredPosition = _targetOffsetPos;

            gameObject.SetActive(true);
        }

        private void MovePanelPauseOn()
        {
            if (_isMovingPanelPause) return;
            _isMovingPanelPause = true;
            _blockBG.SetActive(true);
            _panelPause.DOAnchorPos(_originalPosPanelPause, _duration).SetEase(Ease.OutQuart)
                .OnComplete(() => _isMovingPanelPause = false).SetUpdate(true);
        }

        private void MovePanelPauseOut()
        {
            if (_isMovingPanelPause) return;
            _isMovingPanelPause = true;

            if (_isResumeClick)
            {
                _blockBG.SetActive(false);
                _isResumeClick = false;
            }

            _panelPause.DOAnchorPos(-_targetOffsetPos, _duration).SetEase(Ease.OutQuart)
                .OnComplete(() => _isMovingPanelPause = false).SetUpdate(true);
        }

        private void MovePanelSettingOn()
        {
            if (_isMovingPanelSetting) return;
            _isMovingPanelSetting = true;
            _panelSetting.DOAnchorPos(_originalPosPanelSetting, _duration).SetEase(Ease.OutBack)
                .OnComplete(() => _isMovingPanelSetting = false).SetUpdate(true);
        }

        private void MovePanelSettingOut()
        {
            if (_isMovingPanelSetting) return;
            _isMovingPanelSetting = true;
            _panelSetting.DOAnchorPos(_targetOffsetPos, _duration).SetEase(Ease.OutBack)
                .OnComplete(() => _isMovingPanelSetting = false).SetUpdate(true);
        }

        public void OnPauseClicked()
        {
            Time.timeScale = 0;
            _audioManager.Play(AudioKey.SFX_CLICK_01);
            MovePanelPauseOn();
        }

        public void OnResumeClicked()
        {
            _audioManager.Play(AudioKey.SFX_CLICK_01);
            _isResumeClick = true;
            MovePanelPauseOut();
            Time.timeScale = 1f;
        }

        public void OnSettingClicked()
        {
            _audioManager.Play(AudioKey.SFX_CLICK_01);
            MovePanelSettingOn();
            MovePanelPauseOut();
        }

        public void OnHomeClicked()
        {
            _gameResultSystem.SetEndGame();
            new LoadMainMenuScenceCommand().Execute().Forget();
        }

        public void OnComfirmClicked()
        {
            _audioManager.Play(AudioKey.SFX_CLICK_01);
            MovePanelSettingOut();
            MovePanelPauseOn(); 
        }
    }
}