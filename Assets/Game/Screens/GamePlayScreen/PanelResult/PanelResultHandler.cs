using Cysharp.Threading.Tasks;
using DG.Tweening;
using Sources.Audio;
using Sources.Command;
using Sources.Extension;
using Sources.GamePlaySystem.GameResult;
using Sources.Language;
using Sources.Utils;
using Sources.Utils.Singleton;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;

namespace Game.Screens.GamePlayScreen
{
    public class PanelResultHandler : MonoBehaviour
    {
        private const float _durationZoomIn = 0.4f;
        private const float _durationZoomOut = 0.2f;

        private const int _countCoinIconReward = 5;
        private const float _durationEarnCoin = 0.3f;
        
        private readonly Vector2 _targetScaleStar = new Vector2(1.2f, 1.2f);

        private GameResultSystem _gameResultSystem => Locator<GameResultSystem>.Instance;
        private AudioManager _audioManager => Locator<AudioManager>.Instance;

        private bool _isUserWin;

        [Header("Title")]
        [SerializeField] private LanguageText _title;

        [Header("Reward")]
        [SerializeField] private TMP_Text _textReward;
        [SerializeField] private GameObject _rewardHolder;

        [Header("Playable Director")]
        [SerializeField] private PlayableDirector _playableDirectorPanel;
        [SerializeField] private PlayableDirector _playableDirectorTextReward;

        [Header("Another")]
        [SerializeField] private List<RectTransform> _stars;
        [SerializeField] private Transform _coinTotal;
        [SerializeField] private GameObject _coinIcon;
        [SerializeField] private GameObject _titleTryAgain;
        [SerializeField] private GameObject _blackBG;
        [SerializeField] private GameObject _blockPanel;
        [SerializeField] private GameObject _btnNext;

        public void OnSetUp()
        {
            gameObject.SetActive(false);

            _gameResultSystem.IsUserWin += EndGame;
        }
        
        private void EndGame(bool isUserWin)
        {
            _audioManager.PauseAudio(AudioKey.GAME_PLAY_SONG);

            _blackBG.SetActive(true);
            _isUserWin = isUserWin;
            SetTitle();

            if (isUserWin) SetPanelWin();
            else SetPanelLose();
        }

        private void SetTitle()
        {
            var languagageId = _isUserWin ? LanguageKey.LANGUAGE_TITLE_WIN : LanguageKey.LANGUAGE_TITLE_LOSE;
            _title.OnSetUp(languagageId);
        }

        private async void SetPanelWin()
        {
            _textReward.text = ShortNumber.Get(_gameResultSystem.CoinRewards);
            await UniTask.Delay(1000);

            await AnimationPlayableDirectorPanel();
            await AnimationCollectStars();
            await AnimationPlayableDirectorTextReward();
            await AnimationCollectCoin();

            _blockPanel.SetActive(false);
        }

        private async void SetPanelLose()
        {
            Destroy(_btnNext);
            _rewardHolder.gameObject.SetActive(false);
            _titleTryAgain.gameObject.SetActive(true);

            await UniTask.Delay(1000);
            await AnimationPlayableDirectorPanel();

            _blockPanel.SetActive(false);
        }

        private async UniTask AnimationPlayableDirectorPanel()
        {
            if (_isUserWin) _audioManager.Play(AudioKey.SFX_WIN);
            else _audioManager.Play(AudioKey.SFX_LOSE);

            _playableDirectorPanel.Play();

            var duration = _playableDirectorPanel.duration;
            await UniTask.Delay((int)(duration * 1000));
        }

        private async UniTask AnimationPlayableDirectorTextReward()
        {
            _playableDirectorTextReward.Play();
            var duration = _playableDirectorTextReward.duration;
            await UniTask.Delay((int)(duration * 1000));
        }

        private async UniTask AnimationCollectStars()
        {
            var starCollect = _gameResultSystem.StarWin;

            for (int i = 0; i < starCollect; i++)
            {
                _audioManager.Play(AudioKey.SFX_POP_UP);

                _stars[i].gameObject.SetActive(true);
                _stars[i].localScale = Vector3.zero;
                await _stars[i].DOScale(_targetScaleStar, _durationZoomIn).SetEase(Ease.Linear);
                _stars[i].DOScale(Vector2.one, _durationZoomOut);
            }
        }

        private async UniTask AnimationCollectCoin()
        {
            var coinReward = _gameResultSystem.CoinRewards / _countCoinIconReward;
            for (int i = 0; i < _countCoinIconReward; i++)
            {
                var coin = Instantiate(_coinIcon);
                coin.transform.SetParent(_coinIcon.transform, false);
                coin.transform.position = _coinIcon.transform.position;
                coin.transform.localScale = Vector3.zero;

                var sequence = DOTween.Sequence();

                sequence.Append(coin.transform.DOMove(_coinTotal.transform.position, _durationEarnCoin));
                sequence.Join(coin.transform.DOScale(Vector3.one, _durationEarnCoin));
                sequence.AppendCallback(() =>
                {
                    _gameResultSystem.ClaimReward(coinReward);
                    coin.gameObject.SetActive(false);
                });

                await UniTask.Delay(200);
            }
        }

        private void OnDestroy()
        {
            _playableDirectorPanel.DOKill();
            _playableDirectorTextReward.DOKill();
            _gameResultSystem.IsUserWin -= EndGame;
        }

        public void OnBackHomeClicked()
        {
            _audioManager.Play(AudioKey.SFX_CLICK_01);
            new LoadMainMenuScenceCommand().Execute().Forget();
        }

        public void OnResetClicked()
        {
            _audioManager.Play(AudioKey.SFX_CLICK_01);
            var waveId = _gameResultSystem.WaveIdCurrent;
            new LoadGamePlayScenceCommand(waveId).Execute().Forget();
        }

        public void OnNextWaveClicked()
        {
            _audioManager.Play(AudioKey.SFX_CLICK_01);
            var waveId = _gameResultSystem.WaveIdNext;
            new LoadGamePlayScenceCommand(waveId).Execute().Forget();
        }
    }
}