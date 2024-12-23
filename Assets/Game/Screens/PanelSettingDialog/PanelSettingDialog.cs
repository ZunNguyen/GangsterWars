using Game.Screens.ConfirmDialog;
using Sources.Audio;
using Sources.Command;
using Sources.Extension;
using Sources.Language;
using Sources.UI;
using Sources.UISystem;
using Sources.Utils.Singleton;

namespace Game.Screens.PanelSettingDialog
{
    public class PanelSettingDialog : BaseUI
    {
        private AudioManager _audioManager => Locator<AudioManager>.Instance;
        private LanguageTable _languageTable => Locator<LanguageTable>.Instance;
        private UIManager _uiManager => Locator<UIManager>.Instance;

        public void OnChangeLanguageClicked()
        {
            _audioManager.Play(AudioKey.SFX_CLICK_01);
            _languageTable.ChangeNexLanguageName();
        }

        public async void OnResetDataClicked()
        {
            await _uiManager.Show<ConfirmDialog.ConfirmDialog>();
        }
    }
}