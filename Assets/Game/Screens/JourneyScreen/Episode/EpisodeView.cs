using Sources.GamePlaySystem.JourneyMap;
using Sources.Language;
using Sources.Utils.Singleton;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Screens.JourneyScreen
{
    public class EpisodeView : MonoBehaviour
    {
        private const string _episodeTextDefault = "Episode ";
        private const string _episodeLanguageKey = "Button_Episode";

        private LanguageTable _languageTable => Locator<LanguageTable>.Instance;
        private JourneyMapSystem _journeyMapSystem => Locator<JourneyMapSystem>.Instance;

        private int _indexEpisode;

        [SerializeField] private TMP_Text _text;
        [SerializeField] private GameObject _active;

        public void OnSetUp(int index)
        {
            _indexEpisode = index;

            var num = _indexEpisode + 1;
            var languageItem = _languageTable.GetLanguageItem(_episodeLanguageKey);
            _text.text = languageItem.GetText() + " " + num.ToString();

            var indexEpisodeCurrent = _journeyMapSystem.IndexGridMapMaxCurrent;
            _active.SetActive(_indexEpisode <= indexEpisodeCurrent);
        }

        public void OnChangeEpisodeClicked()
        {
            _journeyMapSystem.OnChangeEpisode(_indexEpisode);
        }
    }
}