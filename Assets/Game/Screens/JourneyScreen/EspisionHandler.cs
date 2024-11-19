using Cysharp.Threading.Tasks;
using DG.Tweening;
using Sources.GamePlaySystem.JourneyMap;
using Sources.Utils.Singleton;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Game.Screens.JourneyScreen
{
    public class EspisionHandler : MonoBehaviour
    {
        private readonly Vector2 _offsetEpisodePos = new Vector2(0, -80f);
        private const int _maxIndexEpision = 3;
        private const int _minEpision = 0;
        private const float _duration = 0.3f;

        private JourneyMapSystem _journeyMapSystem => Locator<JourneyMapSystem>.Instance;

        private bool _isAnimationShow = false;
        private List<RectTransform> _episodesMove = new();

        [SerializeField] private EpisodeView _episodePrefab;
        [SerializeField] private Transform _episodeHolder;

        private void Awake()
        {
            SetEpisodeView();
        }

        private void SetEpisodeView()
        {
            var indexEpisionCurrent = _journeyMapSystem.IndexGridMapCurrent;

            for (int i = _maxIndexEpision; i >= _minEpision; i--)
            {
                if (i != indexEpisionCurrent)
                {
                    var newEpisode = Instantiate(_episodePrefab, _episodeHolder);
                    newEpisode.OnSetUp(i);

                    var rect = newEpisode.GetComponent<RectTransform>();
                    _episodesMove.Add(rect);
                }
            }

            var episodeCurrent = Instantiate(_episodePrefab, _episodeHolder);
            episodeCurrent.OnSetUp(indexEpisionCurrent);
        }

        private async void ShowAllEpisode()
        {
            _isAnimationShow = true;

            var episionsMoveClone = new List<RectTransform>(_episodesMove);
            var countOffset = 1;
            for (int i = episionsMoveClone.Count - 1; i >= 0; i--)
            {
                var targetMove = _offsetEpisodePos * countOffset;

                var moveTasks = new List<Task>();
                foreach (var episode in episionsMoveClone)
                {
                    var task = episode.DOAnchorPos(targetMove, _duration).SetEase(Ease.Linear);
                    moveTasks.Add(task.AsyncWaitForCompletion());
                }

                await Task.WhenAll(moveTasks);
                episionsMoveClone.Remove(episionsMoveClone[i]);
                countOffset++;
            }

            _isAnimationShow = false;
        }

        private async void UnShowAllEpisode()
        {
            _isAnimationShow = true;

            var episionsMoveClone = new List<RectTransform>(_episodesMove);
            var countOffset = episionsMoveClone.Count;
            for (int i = 0; i < episionsMoveClone.Count; i++)
            {
                --countOffset;
                var targetMove = _offsetEpisodePos * countOffset;

                var moveTasks = new List<Task>();
                foreach (var episode in episionsMoveClone)
                {
                    var task = episode.DOAnchorPos(targetMove, _duration).SetEase(Ease.Linear);
                    moveTasks.Add(task.AsyncWaitForCompletion());
                }

                await Task.WhenAll(moveTasks);
                episionsMoveClone.Remove(episionsMoveClone[i]);
            }

            _isAnimationShow = false;
        }

        public void OnShowAllEpisodeClicked()
        {
            if (_isAnimationShow) return;
            ShowAllEpisode();
        }

        public void OnUnShowAllEpisodeClicked()
        {
            if (_isAnimationShow) return;
            UnShowAllEpisode();
        }
    }
}