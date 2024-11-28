using Sources.Extension;
using Sources.GameData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sources.GameData
{
    public class UserSettingProfile : IProfileData
    {
        public string LanguageName { get; private set; }
        public float MusicVolume { get; private set; }
        public float SFXVolume { get; private set; }

        public UserSettingProfile()
        {
            LanguageName = LanguageKey.LANGUAGE_VIETNAME;
            MusicVolume = 0.5f;
            SFXVolume = 0.5f;
        }

        public void SetLanguageName(string languageName)
        {
            LanguageName = languageName;
        }

        public void SetMusicVolume(float value)
        {
            MusicVolume = value;
        }

        public void SetSFXVolume(float value)
        {
            SFXVolume = value;
        }
    }
}