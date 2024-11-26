using Cysharp.Threading.Tasks;
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

        public InitAudioManagerService(AudioObjectInstance audioObjectInstance)
        {
            _audioObjectInstance = audioObjectInstance;
        }

        public override async UniTask<IService.Result> Execute()
        {
            var audioManager = new AudioManager();
            audioManager.Init(_audioObjectInstance);

            Locator<AudioManager>.Set(audioManager);
            return IService.Result.Success;
        }
    }
}