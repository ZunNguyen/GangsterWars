using Sources.DataBaseSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Sources.UI;
using Sources.Language;
using Sources.Utils.Singleton;
using Sources.Audio;
using UnityEngine.UI;

namespace Game.Screens.PanelSettingDialog
{
    public class PanelSettingDialog : BaseUI
    {
        private AudioManager _audioManager => Locator<AudioManager>.Instance;
        private LanguageTable _languageTable => Locator<LanguageTable>.Instance;

        [SerializeField] private Slider _sliderMusic;
        [SerializeField] private Slider _sliderSFX;

        public void OnChangeLanguageClicked()
        {
            _languageTable.ChangeNexLanguageName();
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