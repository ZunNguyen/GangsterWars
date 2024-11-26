using Sources.DataBaseSystem;
using Sources.SpawnerSystem;
using Sources.Utils.Singleton;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sources.Audio
{
    public class AudioManager
    {
        private DataBase _dataBase => Locator<DataBase>.Instance;
        private AudioConfig _audioConfig => _dataBase.GetConfig<AudioConfig>();
        private SpawnerManager _spawnerManager => Locator<SpawnerManager>.Instance;

        private AudioObjectInstance _audioObjectInstance;
        private GameObject _audioHolder;

        public void Init(AudioObjectInstance audioObjectInstance)
        {
            _audioObjectInstance = audioObjectInstance;

            _audioHolder = new GameObject("Audio Holder");
            _audioHolder.transform.position = Vector3.zero;
            Object.DontDestroyOnLoad(_audioHolder);
        }

        public void Play(string audioId, bool isLoop)
        {
            if (string.IsNullOrEmpty(audioId)) return;

            var audioInfo = _audioConfig.GetAudioInfo(audioId);
            var audioObj = GetAudioObject();
            audioObj.OnSetUp(audioInfo, isLoop);
        }

        private AudioObjectInstance GetAudioObject()
        {
            var audioObject = _spawnerManager.Get(_audioObjectInstance);
            audioObject.transform.SetParent(_audioHolder.transform, false);
            return audioObject;
        }
    }
}