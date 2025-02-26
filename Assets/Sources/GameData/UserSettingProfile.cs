using Sources.Extension;
using Sources.GameData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sources.GameData
{
    public class UserSettingProfile : IProfileData
    {
        public string LanguageName = LanguageKey.LANGUAGE_VIETNAME;
        public float MusicVolume = 0.5f;
        public float SFXVolume = 0.5f;

#if UNITY_ANDROID || UNITY_IOS
        public float CursorSensitivity = 7f;
#endif

        public void SetLanguageName(string languageName)
        {
            LanguageName = languageName;
            Save();
        }

        public void SetMusicVolume(float value)
        {
            MusicVolume = value;
            Save();
        }

        public void SetSFXVolume(float value)
        {
            SFXVolume = value;
            Save();
        }

#if UNITY_ANDROID || UNITY_IOS
        public void SetCursorSensitivity(float value)
        {
            CursorSensitivity = value;
            Save();
        }
#endif
    }
}