using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Screens.MainMenuScreen
{
    public enum TabState
    {
        TabGun,
        TabBom,
        TabShield
    }

    public class TabHandler : MonoBehaviour
    {
        [SerializeField] private TabView _tabGun;
        [SerializeField] private TabView _tabBom;
        [SerializeField] private TabView _tabShield;

        public Action<TabState> TabStateChange;

        public async void OnSetUp()
        {
            _tabGun.OnSetUp(TabState.TabGun);
            _tabBom.OnSetUp(TabState.TabBom);
            _tabShield.OnSetUp(TabState.TabShield);

            await UniTask.DelayFrame(1);
            TabStateChange?.Invoke(TabState.TabGun);
        }

        public void OnChangeTabState(TabState state)
        {
            TabStateChange?.Invoke(state);
        }
    }
}