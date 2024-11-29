using Cysharp.Threading.Tasks;
using Sources.Audio;
using Sources.Extension;
using Sources.UI;
using Sources.Utils.Singleton;

namespace Game.Screens.JourneyScreen
{
    public class JourneyScreen : BaseUI
    {
        private AudioManager _audioManager => Locator<AudioManager>.Instance;

        public override void Back()
        {
            _audioManager.Play(AudioKey.SFX_CLICK_01);
            base.Back();
        }
    }
}