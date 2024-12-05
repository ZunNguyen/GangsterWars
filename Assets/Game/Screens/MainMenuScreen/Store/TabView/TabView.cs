using Sources.Audio;
using Sources.Extension;
using Sources.GamePlaySystem.MainMenuGame;
using Sources.Utils.Singleton;
using UniRx;
using UnityEngine;

namespace Game.Screens.MainMenuScreen
{
    public class TabView : MonoBehaviour
    {
        private StoreSystem _storeSystem => Locator<StoreSystem>.Instance;
        private AudioManager _audioManager => Locator<AudioManager>.Instance;

        private bool _isOpened = false;
        private TabState _tabStateCurrent;

        [SerializeField] private GameObject _notSelected;

        public void OnSetUp(TabState state, bool isOpened = false)
        {
            _tabStateCurrent = state;
            _isOpened = isOpened;

            _storeSystem.TabCurrent.Subscribe(UpdateTabState).AddTo(this);
        }

        public void UpdateOpenStore(bool isOpen)
        {
            _isOpened = isOpen;
            _storeSystem.SetTabCurrent(_tabStateCurrent);
        }

        public void OnClicked()
        {
            if (!_isOpened)
            {
                _audioManager.Play(AudioKey.SFX_CLICK_ERROR);
                return;
            }

            _audioManager.Play(AudioKey.SFX_CLICK_01);
            _storeSystem.SetTabCurrent(_tabStateCurrent);
        }

        private void UpdateTabState(TabState tabState)
        {
            _notSelected.SetActive(tabState != _tabStateCurrent);
        }
    }
}