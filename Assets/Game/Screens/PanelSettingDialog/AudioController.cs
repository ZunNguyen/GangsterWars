using Sources.Audio;
using Sources.Utils.Singleton;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

namespace Game.Screens.PanelSettingDialog
{
    public class AudioController : MonoBehaviour
    {
        private AudioManager _audioManager => Locator<AudioManager>.Instance;

        [SerializeField] private Slider _sliderMusic;
        [SerializeField] private Slider _sliderSFX;

        private void Start()
        {
            _audioManager.MusicVolume.Subscribe(value =>
            {
                _sliderMusic.value = value;
            }).AddTo(this);

            _audioManager.SFXVolume.Subscribe(value =>
            {
                _sliderSFX.value = value;
            }).AddTo(this);
        }

        public void OnMusicVolumeChanged()
        {
            _audioManager.AdjustMusicVolume(_sliderMusic.value);
        }

        public void OnSFXVolumeChanged()
        {
            _audioManager.AdjustSFXVolume(_sliderSFX.value);
        }
    }
}