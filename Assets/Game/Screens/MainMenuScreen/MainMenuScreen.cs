using Cysharp.Threading.Tasks;
using Sources.Audio;
using Sources.Command;
using Sources.Extension;
using Sources.GamePlaySystem.CoinController;
using Sources.UI;
using Sources.UISystem;
using Sources.Utils.Singleton;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Screens.MainMenuScreen
{
    public class MainMenuScreen : BaseUI
    {
        private UIManager _uiManager => Locator<UIManager>.Instance;
        private CoinControllerSystem _coinControllerSystem => Locator<CoinControllerSystem>.Instance;
        private AudioManager _audioManager => Locator<AudioManager>.Instance;

        [SerializeField] private Text _text;
        [SerializeField] private StoreController _storeController;
        [SerializeField] private TabHandler _tabHandler;

        protected override void Awake()
        {
            base.Awake();
            _coinControllerSystem.Coins.Subscribe(value =>
            {
                _text.text = value.ToString();
            }).AddTo(this);

            _storeController.OnSetUp();
            _tabHandler.OnSetUp();
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