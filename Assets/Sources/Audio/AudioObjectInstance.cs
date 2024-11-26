using Cysharp.Threading.Tasks;
using Sources.DataBaseSystem;
using Sources.SpawnerSystem;
using Sources.Utils.Singleton;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sources.Audio
{
    public class AudioObjectInstance : MonoBehaviour
    {
        private SpawnerManager _spawnerManager => Locator<SpawnerManager>.Instance;

        private string _audioId;
        private bool _isMusic;

        [SerializeField] AudioSource _audioSource;

        public async void OnSetUp(AudioInfo audioInfo, bool isLoop)
        {
            _audioId = audioInfo.Id;
            _isMusic =  audioInfo.IsMusic;
            _audioSource.loop = isLoop;

            _audioSource.Play();
            if (!isLoop)
            {
                var lengthAudio = (int)(_audioSource.clip.length * 1000);
                await UniTask.Delay(lengthAudio);
                _spawnerManager.Release(this);
            }
        }
    }
}