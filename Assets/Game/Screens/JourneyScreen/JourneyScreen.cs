using Cysharp.Threading.Tasks;
using Sources.Audio;
using Sources.Extension;
using Sources.FTUE.System;
using Sources.UI;
using Sources.Utils.Singleton;

namespace Game.Screens.JourneyScreen
{
    public class JourneyScreen : BaseUI
    {
        private AudioManager _audioManager => Locator<AudioManager>.Instance;
        private FTUESystem _ftueSystem => Locator<FTUESystem>.Instance;

        private void Start()
        {
            _ftueSystem.TriggerWaitPoint(FTUEKey.WaitPoint_OpenJM);
        }

        public override void Back()
        {
            _audioManager.Play(AudioKey.SFX_CLICK_01);
            base.Back();
        }
    }
}