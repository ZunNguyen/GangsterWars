using BestHTTP.SecureProtocol.Org.BouncyCastle.X509;
using Cysharp.Threading.Tasks;
using Sources.GamePlaySystem.MainMenuGame;
using Sources.Utils.Singleton;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace Game.Screens.MainMenuScreen
{
    public enum TabState
    {
        TabGun,
        TabBom,
        TabSniper,
        TabShield
    }

    public class TabHandler : MonoBehaviour
    {
        private StoreSystem _storeSystem => Locator<StoreSystem>.Instance;

        [SerializeField] private TabView _tabGun;
        [SerializeField] private TabView _tabBom;
        [SerializeField] private TabView _tabShield;

        public Action<TabState> TabStateChange;

        public async void OnSetUp()
        {
            _tabGun.OnSetUp(TabState.TabGun, true);
            _tabBom.OnSetUp(TabState.TabBom, false);
            _tabShield.OnSetUp(TabState.TabShield, true);

            _storeSystem.OnpenBomberStore.Subscribe(SubOpenBomberStore).AddTo(this);

            await UniTask.DelayFrame(5);
            TabStateChange?.Invoke(TabState.TabGun);
        }

        private void SubOpenBomberStore(bool isOpen)
        {
            _tabBom.UpdateOpenStore(isOpen);
        }

        public void OnChangeTabState(TabState state)
        {
            TabStateChange?.Invoke(state);
        }
    }
}