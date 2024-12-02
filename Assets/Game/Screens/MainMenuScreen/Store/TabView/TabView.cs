using Sources.Audio;
using Sources.Extension;
using Sources.Utils.Singleton;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Screens.MainMenuScreen
{
    public class TabView : MonoBehaviour
    {
        private AudioManager _audioManager => Locator<AudioManager>.Instance;

        private bool _isOpened = false;
        private TabState _tabStateCurrent;

        [SerializeField] private TabHandler _tabHandler;
        [SerializeField] private GameObject _notSelected;

        public void OnSetUp(TabState state, bool isOpened = false)
        {
            _tabStateCurrent = state;
            _isOpened = isOpened;

            _tabHandler.TabStateChange += UpdateTabState;
        }

        public void UpdateOpenStore(bool isOpen)
        {
            _isOpened = isOpen;
            _tabHandler.OnChangeTabState(_tabStateCurrent);
        }

        public void OnClicked()
        {
            if (!_isOpened)
            {
                _audioManager.Play(AudioKey.SFX_CLICK_ERROR);
                return;
            }

            _audioManager.Play(AudioKey.SFX_CLICK_01);
            _tabHandler.OnChangeTabState(_tabStateCurrent);
        }

        private void UpdateTabState(TabState tabState)
        {
            _notSelected.SetActive(tabState != _tabStateCurrent);
        }

        private void OnDestroy()
        {
            _tabHandler.TabStateChange -= UpdateTabState;
        }
    }
}