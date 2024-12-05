using Cysharp.Threading.Tasks;
using Sources.GamePlaySystem.MainMenuGame;
using Sources.Utils.Singleton;
using System;
using UniRx;
using UnityEngine;

namespace Game.Screens.MainMenuScreen
{
    public class TabHandler : MonoBehaviour
    {
        private StoreSystem _storeSystem => Locator<StoreSystem>.Instance;

        [SerializeField] private TabView _tabGun;
        [SerializeField] private TabView _tabBom;
        [SerializeField] private TabView _tabSniper;
        [SerializeField] private TabView _tabShield;

        public async void OnSetUp()
        {
            _tabGun.OnSetUp(TabState.TabGun, true);
            _tabBom.OnSetUp(TabState.TabBom);
            _tabSniper.OnSetUp(TabState.TabSniper);
            _tabShield.OnSetUp(TabState.TabShield, true);

            _storeSystem.OpenBomberStore.Subscribe(SubOpenBomberStore).AddTo(this);
            _storeSystem.OpenSniperStore.Subscribe(SubOpenSniperStore).AddTo(this);

            await UniTask.DelayFrame(5);
            _storeSystem.SetTabCurrent(TabState.TabGun);
        }

        private void SubOpenBomberStore(bool isOpen)
        {
            _tabBom.UpdateOpenStore(isOpen);
        }

        private void SubOpenSniperStore(bool isOpen)
        {
            _tabSniper.UpdateOpenStore(isOpen);
        }
    }
}