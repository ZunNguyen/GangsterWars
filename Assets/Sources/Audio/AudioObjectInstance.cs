using Cysharp.Threading.Tasks;
using Sources.DataBaseSystem;
using Sources.SpawnerSystem;
using Sources.Utils.Singleton;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace Sources.Audio
{
    public class AudioObjectInstance : MonoBehaviour
    {
        private SpawnerManager _spawnerManager => Locator<SpawnerManager>.Instance;
        private AudioManager _audioManager => Locator<AudioManager>.Instance;

        private string _audioId;
        private bool _isMusic;

        private IDisposable _disposabelMusicVolume;

        [SerializeField] AudioSource _audioSource;

        private void OnEnable()
        {
            SubscriseMusicVolume();
        }

        private void OnDisable()
        {
            _disposabelMusicVolume?.Dispose();
        }

        private void OnDestroy()
        {
            OnDisable();
        }

        public async void OnSetUp(AudioInfo audioInfo, bool isLoop)
        {
            _audioId = audioInfo.Id;
            _isMusic =  audioInfo.IsMusic;
            _audioSource.loop = isLoop;

            _audioSource.clip = audioInfo.AudioClip;
            _audioSource.Play();
            if (!isLoop)
            {
                var lengthAudio = (int)(_audioSource.clip.length * 1000);
                await UniTask.Delay(lengthAudio);
                _spawnerManager.Release(this);
            }
        }

        private void SubscriseMusicVolume()
        {
            _disposabelMusicVolume = _audioManager.MusicVolume.Subscribe(value =>
            {
                _audioSource.volume = value;
            }).AddTo(this);
        }
    }
}