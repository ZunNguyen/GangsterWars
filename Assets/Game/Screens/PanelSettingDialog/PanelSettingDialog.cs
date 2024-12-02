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
using Sources.Extension;
using Sources.Command;

namespace Game.Screens.PanelSettingDialog
{
    public class PanelSettingDialog : BaseUI
    {
        private AudioManager _audioManager => Locator<AudioManager>.Instance;
        private LanguageTable _languageTable => Locator<LanguageTable>.Instance;

        [SerializeField] private Slider _sliderMusic;
        [SerializeField] private Slider _sliderSFX;

        public void OnMusicVolumeChanged()
        {
            _audioManager.AdjustMusicVolume(_sliderMusic.value);
        }

        public void OnSFXVolumeChanged()
        {
            _audioManager.AdjustSFXVolume(_sliderSFX.value);
        }

        public void OnChangeLanguageClicked()
        {
            _audioManager.Play(AudioKey.SFX_CLICK_01);
            _languageTable.ChangeNexLanguageName();
        }

        public void OnResetDataClicked()
        {
            _audioManager.Play(AudioKey.SFX_CLICK_01);
            new ResetDataCommand().Execute();
        }
    }
}