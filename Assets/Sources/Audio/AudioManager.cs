using Sources.DataBaseSystem;
using Sources.GameData;
using Sources.SpawnerSystem;
using Sources.Utils;
using Sources.Utils.Singleton;
using System;
using UniRx;
using UnityEngine;

namespace Sources.Audio
{
    public class AudioManager
    {
        private GameData.GameData _gameData => Locator<GameData.GameData>.Instance;
        private UserSettingProfile _userSettingProfile => _gameData.GetProfileData<UserSettingProfile>();

        private AudioData _audioConfig => Locator<AudioData>.Instance;
        private SpawnerManager _spawnerManager => Locator<SpawnerManager>.Instance;

        private AudioObjectInstance _audioObjectInstance;
        private GameObject _audioHolder;

        public ReactiveProperty<float> MusicVolume = new();
        public ReactiveProperty<float> SFXVolume = new();
        public Action<string> OnPause;
        public Action AllAudioPause;

        public void Init(AudioObjectInstance audioObjectInstance)
        {
            _audioObjectInstance = audioObjectInstance;

            _audioHolder = new GameObject("Audio Holder");
            _audioHolder.transform.position = Vector3.zero;
            UnityEngine.Object.DontDestroyOnLoad(_audioHolder);

            LoadSetting();
        }

        private void LoadSetting()
        {
            MusicVolume.Value = _userSettingProfile.MusicVolume;
            SFXVolume.Value = _userSettingProfile.SFXVolume;
        }

        public void Play(string audioId, bool isLoop = false)
        {
            if (string.IsNullOrEmpty(audioId)) return;

            var audioInfo = _audioConfig.GetAudioInfo(audioId);
            var audioObj = GetAudioObject();
            audioObj.OnSetUp(audioInfo, isLoop);
        }

        private void PlayAudio(string audioId, bool isLoop = false)
        {

        }

        private void PlayAudioList(string audioId, bool isLoop = false)
        {

        }

        public void PauseAudio(string audioId)
        {
            OnPause?.Invoke(audioId);
        }

        public void AllPauseAudio()
        {
            AllAudioPause?.Invoke();
        }

        private AudioObjectInstance GetAudioObject()
        {
            var audioObject = _spawnerManager.Get(_audioObjectInstance);
            audioObject.transform.SetParent(_audioHolder.transform, false);
            return audioObject;
        }

        public void AdjustMusicVolume(float value)
        {
            MusicVolume.Value = value;
        }

        public void AdjustSFXVolume(float value)
        {
            SFXVolume.Value = value;
        }
    }
}