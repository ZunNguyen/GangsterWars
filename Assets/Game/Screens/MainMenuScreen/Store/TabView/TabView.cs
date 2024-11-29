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

        private TabState _tabStateCurrent;

        [SerializeField] private TabHandler _tabHandler;
        [SerializeField] private GameObject _notSelected;

        public void OnSetUp(TabState state)
        {
            _tabStateCurrent = state;

            _tabHandler.TabStateChange += UpdateTabState;
        }

        public void OnClicked()
        {
            _audioManager.Play(AudioKey.SFX_CLICK_01);
            _tabHandler.OnChangeTabState(_tabStateCurrent);
        }

        private void UpdateTabState(TabState tabState)
        {
            if (tabState != _tabStateCurrent)
            {
                _notSelected.SetActive(true);
            }
            else
            {
                _notSelected.SetActive(false);
            }
        }

        private void OnDestroy()
        {
            _tabHandler.TabStateChange -= UpdateTabState;
        }
    }
}