using Sources.Audio;
using Sources.Extension;
using Sources.GamePlaySystem.MainMenuGame;
using Sources.Utils.Singleton;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Screens.MainMenuScreen
{
    public class TabCoin : MonoBehaviour
    {
        private StoreSystem _storeSystem => Locator<StoreSystem>.Instance;
        private AudioManager _audioManager => Locator<AudioManager>.Instance;

        private bool _isOpened = false;
        private TabState _tabStateCurrent;

        [SerializeField] private GameObject _notSelected;

        private void Awake()
        {
            _tabStateCurrent = TabState.TabCoin;
        }

        public void OnClicked()
        {
            _audioManager.Play(AudioKey.SFX_CLICK_01);
            _storeSystem.SetTabCurrent(_tabStateCurrent);
        }
    }
}