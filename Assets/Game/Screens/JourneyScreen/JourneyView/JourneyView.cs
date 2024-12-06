using DG.Tweening;
using Sources.Audio;
using Sources.Extension;
using Sources.GameData;
using Sources.GamePlaySystem.JourneyMap;
using Sources.Utils.Singleton;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Game.Screens.JourneyScreen
{
    public class JourneyView : MonoBehaviour
    {
        private readonly Vector3 _targetScale = new Vector3(1.1f, 1.1f, 1f);
        private const float _duration = 0.7f;

        private GameData _gameData => Locator<GameData>.Instance;
        private JourneyProfile _journeyProfile => _gameData.GetProfileData<JourneyProfile>();
        private JourneyMapSystem _journeyMapSystem => Locator<JourneyMapSystem>.Instance;
        private AudioManager _audioManager => Locator<AudioManager>.Instance;

        private string _waveId;
        private bool _isCanClick = true;

        [SerializeField] private TMP_Text _waveText;
        [SerializeField] private List<GameObject> _stars;
        [SerializeField] private GameObject _lock;
        [SerializeField] private GameObject _starsHolder;
        [SerializeField] private RectTransform _rect;

        private void Awake()
        {
            foreach (var star in _stars)
            {
                star.gameObject.SetActive(false);
            }
            _lock.SetActive(false);
        }

        public void OnSetUp(string waveId)
        {
            _waveId = waveId;

            var journeyItemState = _journeyMapSystem.GetJourneyItemState(waveId);

            if (journeyItemState == JourneyItemState.Passed)
            {
                var waveData = _journeyProfile.GetWaveData(waveId);
                _waveText.text = waveId;
                for (int i = 0; i < waveData.Stars; i++)
                {
                    _stars[i].SetActive(true);
                }
            }
            if (journeyItemState == JourneyItemState.NotYetPass)
            {
                _waveText.text = waveId;
                ShowAnimation();
            }
            else if (journeyItemState == JourneyItemState.Lock)
            {
                _isCanClick = false;
                _waveText.gameObject.SetActive(false);
                _lock.SetActive(true);
                _starsHolder.SetActive(false);
            }
        }

        private void ShowAnimation()
        {
            var sequence = DOTween.Sequence();
            sequence.Append(_rect.DOScale(_targetScale, _duration).SetEase(Ease.Linear))
                    .Append(_rect.DOScale(Vector3.one, _duration).SetEase(Ease.Linear))
                    .SetLoops(-1);
            sequence.Play();
        }

        public void OnBattleWaveClicked()
        {
            if (!_isCanClick)
            {
                _audioManager.Play(AudioKey.SFX_CLICK_ERROR);
                return;
            }
            _audioManager.Play(AudioKey.SFX_CLICK_01);
            _journeyMapSystem.OnBattleWave(_waveId);
        }
    }
}