using Sources.Command;
using Sources.GameData;
using Sources.GamePlaySystem.JourneyMap;
using Sources.GamePlaySystem.MainGamePlay;
using Sources.Utils.Singleton;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Game.Screens.JourneyScreen
{
    public class JourneyView : MonoBehaviour
    {
        private GameData _gameData => Locator<GameData>.Instance;
        private JourneyProfile _journeyProfile => _gameData.GetProfileData<JourneyProfile>();
        private JourneyMapSystem _journeyMapSystem => Locator<JourneyMapSystem>.Instance;

        private string _waveId;
        private bool _isCanClick = true;

        [SerializeField] private TMP_Text _waveText;
        [SerializeField] private List<GameObject> _stars;
        [SerializeField] private GameObject _lock;
        [SerializeField] private GameObject _starsHolder;

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
                for (int i = 0; i < waveData.Stars; i++)
                {
                    _stars[i].SetActive(true);
                }
            }
            else if (journeyItemState == JourneyItemState.Lock)
            {
                _isCanClick = false;
                _waveText.gameObject.SetActive(false);
                _lock.SetActive(true);
                _starsHolder.SetActive(false);
            }
        }

        public void OnBattleWaveClicked()
        {
            if (!_isCanClick) return;
            _journeyMapSystem.OnBattleWave(_waveId);
        }
    }
}