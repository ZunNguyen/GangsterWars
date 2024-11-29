using Cysharp.Threading.Tasks;
using Game.Screens.GamePlayScreen;
using Sources.Audio;
using Sources.UISystem;
using Sources.Utils.Singleton;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Sources.Command
{
    public class LoadSenceCommand : Command
    {
        private readonly string _scenceName;
        private AudioManager _audioManager => Locator<AudioManager>.Instance;

        public LoadSenceCommand(string sceneName)
        {
            _scenceName = sceneName;
        }

        public override async UniTask Execute()
        {
            _audioManager.AllPauseAudio();
            await UniTask.DelayFrame(3);
            await new ResetSpawnerManagerCommand().Execute();
            await SceneManager.LoadSceneAsync(_scenceName, LoadSceneMode.Single);
        }
    }
}