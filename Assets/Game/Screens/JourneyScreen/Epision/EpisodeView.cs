using Sources.GamePlaySystem.JourneyMap;
using Sources.Utils.Singleton;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Screens.JourneyScreen
{
    public class EpisodeView : MonoBehaviour
    {
        private const string _episodeTextDefault = "Episode ";

        private JourneyMapSystem _journeyMapSystem => Locator<JourneyMapSystem>.Instance;

        private int _indexEpisode;

        [SerializeField] private Text _text;
        [SerializeField] private GameObject _active;

        public void OnSetUp(int index)
        {
            _indexEpisode = index;

            var num = _indexEpisode + 1;
            _text.text = _episodeTextDefault + num.ToString();

            var indexEpisodeCurrent = _journeyMapSystem.IndexGridMapMaxCurrent;
            _active.SetActive(_indexEpisode <= indexEpisodeCurrent);
        }

        public void OnChangeEpisodeClicked()
        {
            _journeyMapSystem.OnChangeEpisode(_indexEpisode);
        }
    }
}