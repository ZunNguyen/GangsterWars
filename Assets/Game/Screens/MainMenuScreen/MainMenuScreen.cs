using Cysharp.Threading.Tasks;
using Sources.Audio;
using Sources.Extension;
using Sources.GamePlaySystem.CoinController;
using Sources.UI;
using Sources.UISystem;
using Sources.Utils;
using Sources.Utils.Singleton;
using TMPro;
using UniRx;
using UnityEngine;

namespace Game.Screens.MainMenuScreen
{
    public class MainMenuScreen : BaseUI
    {
        private UIManager _uiManager => Locator<UIManager>.Instance;
        private AudioManager _audioManager => Locator<AudioManager>.Instance;

        [SerializeField] private StoreController _storeController;
        [SerializeField] private TabHandler _tabHandler;

        protected override void Awake()
        {
            base.Awake();

            _storeController.OnSetUp();
            _tabHandler.OnSetUp();

            _audioManager.Play(AudioKey.MENU_SONG, true);
        }

        public async void OnPlayGameClicked()
        {
            _audioManager.Play(AudioKey.SFX_CLICK_01);
            await _uiManager.Show<JourneyScreen.JourneyScreen>();
        }

        public async void OnSettingClicked()
        {
            _audioManager.Play(AudioKey.SFX_CLICK_01);
            await _uiManager.Show<PanelSettingDialog.PanelSettingDialog>();
        }
    }
}