using Cysharp.Threading.Tasks;
using Sources.Audio;
using Sources.Extension;
using Sources.FTUE.System;
using Sources.Utils.Singleton;
using UnityEngine.SceneManagement;

namespace Sources.Command
{
    public class LoadSenceCommand : Command
    {
        private readonly string _scenceName;
        private AudioManager _audioManager => Locator<AudioManager>.Instance;
        private FTUESystem _ftueSystem => Locator<FTUESystem>.Instance;

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

            _ftueSystem.TriggerWaitPoint(FTUEKey.WaitPoint_FinishOpenMainMenuScreen);
        }
    }
}