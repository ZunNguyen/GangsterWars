using Cysharp.Threading.Tasks;
using Sources.DataBaseSystem;
using Sources.SpawnerSystem;
using Sources.Utils.Singleton;
using System;
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

        [SerializeField] AudioSource _audioSource;

        private void Awake()
        {
            SubscriseEvent();
        }

        private void SubscriseEvent()
        {
            _audioManager.MusicVolume.Subscribe(UpdateMusicVolume).AddTo(this);
            _audioManager.SFXVolume.Subscribe(UpdateSFXVolume).AddTo(this);
            _audioManager.OnPause += Pause;
            _audioManager.AllAudioPause += AllPause;
        }

        private void UpdateMusicVolume(float value)
        {
            if (_isMusic) _audioSource.volume = value;
        }

        private void UpdateSFXVolume(float value)
        {
            if (!_isMusic) _audioSource.volume = value;
        }

        private void Pause(string id)
        {
            if (_audioId != id) return;
            if (gameObject.activeSelf)
                _spawnerManager.Release(gameObject);
        }

        private void AllPause()
        {
            if (gameObject.activeSelf)
                _spawnerManager.Release(gameObject);
        }

        public async void OnSetUp(AudioInfo audioInfo, bool isLoop)
        {
            _audioId = audioInfo.Id;
            _isMusic = audioInfo.IsMusic;
            _audioSource.loop = isLoop;

            if (_isMusic) _audioSource.volume = _audioManager.MusicVolume.Value;

            _audioSource.clip = audioInfo.TakeRandom();
            _audioSource.Play();

            if (!isLoop)
            {
                var token = this.GetCancellationTokenOnDestroy();
                try
                {
                    var lengthAudio = (int)(_audioSource.clip.length * 1000);
                    await UniTask.Delay(lengthAudio, cancellationToken : token);
                    if (gameObject.activeSelf) _spawnerManager.Release(gameObject);
                }
                catch (OperationCanceledException) { }
            }
        }

        private void OnDestroy()
        {
            _audioManager.OnPause -= Pause;
            _audioManager.AllAudioPause -= AllPause;
        }
    }
}