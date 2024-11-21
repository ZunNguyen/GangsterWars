using Cysharp.Threading.Tasks;
using DG.Tweening;
using Sources.Command;
using Sources.GamePlaySystem.GameResult;
using Sources.GamePlaySystem.MainGamePlay;
using Sources.Utils.Singleton;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

namespace Game.Screens.GamePlayScreen
{
    public class PanelResultHandler : MonoBehaviour
    {
        private const float _durationZoomIn = 0.5f;
        private const float _durationZoomOut = 0.2f;
        private readonly Vector2 _targetScaleStar = new Vector2(1.2f, 1.2f);

        private GameResultSystem _gameResultSystem => Locator<GameResultSystem>.Instance;

        [Header("Text")]
        [SerializeField] private Text _textTitle;
        [SerializeField] private Text _textMoney;

        [Header("Another")]
        [SerializeField] private List<RectTransform> _stars;
        [SerializeField] private GameObject _titleTryAgain;
        [SerializeField] private GameObject _rewardHolder;
        [SerializeField] private GameObject _blackBG;
        [SerializeField] private GameObject _blockPanel;
        [SerializeField] private GameObject _btnNext;
        [SerializeField] private PlayableDirector _playableDirector;

        public void OnSetUp()
        {
            gameObject.SetActive(false);

            _gameResultSystem.IsUserWin += EndGame;
        }
        
        private void EndGame(bool isUserWin)
        {
            _blackBG.SetActive(true);

            if (isUserWin) SetPanelWin();
            else SetPanelLose();
        }

        private async void SetPanelWin()
        {
            await UniTask.Delay(1000);
            await AnimationPlayableDirector();
            await AnimationCollectStars();

            _blockPanel.SetActive(false);
        }

        private async void SetPanelLose()
        {
            _textTitle.text = "You Lose";
            Destroy(_btnNext);
            _rewardHolder.gameObject.SetActive(false);
            _titleTryAgain.gameObject.SetActive(true);

            await UniTask.Delay(1000);
            await AnimationPlayableDirector();

            _blockPanel.SetActive(false);
        }

        private async UniTask AnimationPlayableDirector()
        {
            _playableDirector.Play();
            await UniTask.Delay(1500);
        }

        private async UniTask AnimationCollectStars()
        {
            var starCollect = _gameResultSystem.StarWin;

            for (int i = 0; i < starCollect; i++)
            {
                _stars[i].gameObject.SetActive(true);
                _stars[i].localScale = Vector3.zero;
                await _stars[i].DOScale(_targetScaleStar, _durationZoomIn).SetEase(Ease.Linear);
                _stars[i].DOScale(Vector2.one, _durationZoomOut);
            }
        }

        private void OnDestroy()
        {
            _gameResultSystem.IsUserWin -= EndGame;
        }

        public void OnBackHomeClicked()
        {
            new LoadMainMenuScenceCommand().Execute().Forget();
        }

        public void OnResetClicked()
        {
            var waveId = _gameResultSystem.WaveIdCurrent;
            new LoadGamePlayScenceCommand(waveId).Execute().Forget();
        }

        public void OnNextWaveClicked()
        {
            var waveId = _gameResultSystem.WaveIdNext;
            new LoadGamePlayScenceCommand(waveId).Execute().Forget();
        }
    }
}