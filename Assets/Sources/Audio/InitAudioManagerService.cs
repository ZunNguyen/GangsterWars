using Cysharp.Threading.Tasks;
using Sources.DataBaseSystem;
using Sources.Services;
using Sources.SystemService;
using Sources.Utils.Singleton;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sources.Audio
{
    public class InitAudioManagerService : Service
    {
        private readonly AudioObjectInstance _audioObjectInstance;
        private readonly AudioData _audioData;

        public InitAudioManagerService(AudioData audioData, AudioObjectInstance audioObjectInstance)
        {
            _audioData = audioData;
            _audioObjectInstance = audioObjectInstance;
        }

        public override async UniTask<IService.Result> Execute()
        {
            Locator<AudioData>.Set(_audioData);

            var audioManager = new AudioManager();
            audioManager.Init(_audioObjectInstance);

            Locator<AudioManager>.Set(audioManager);
            return IService.Result.Success;
        }
    }
}