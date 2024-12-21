using Cysharp.Threading.Tasks;
using Sources.Audio;
using Sources.Command;
using Sources.Extension;
using Sources.Language;
using Sources.UI;
using Sources.Utils.Singleton;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

namespace Game.Screens.PanelSettingDialog
{
    public class PanelSettingDialog : BaseUI
    {
        private AudioManager _audioManager => Locator<AudioManager>.Instance;
        private LanguageTable _languageTable => Locator<LanguageTable>.Instance;

        public void OnChangeLanguageClicked()
        {
            _audioManager.Play(AudioKey.SFX_CLICK_01);
            _languageTable.ChangeNexLanguageName();
        }

        public void OnResetDataClicked()
        {
            _audioManager.Play(AudioKey.SFX_CLICK_01);
            new ResetDataCommand().Execute();
        }
    }
}