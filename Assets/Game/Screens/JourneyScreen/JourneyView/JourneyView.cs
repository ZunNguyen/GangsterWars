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

        [SerializeField] private TMP_Text _waveText;
        [SerializeField] private List<GameObject> _stars;

        private void Awake()
        {
            foreach (var star in _stars)
            {
                star.gameObject.SetActive(false);
            }
        }

        public void OnSetUp(string waveId)
        {
            _waveId = waveId;

            var waveData = _journeyProfile.GetWaveData(waveId);
            for (int i = 0; i < waveData.Stars; i++)
            {
                _stars[i].SetActive(true);
            }
        }

        public void OnBattleWaveClicked()
        {
            _journeyMapSystem.OnBattleWave(_waveId);
        }
    }
}