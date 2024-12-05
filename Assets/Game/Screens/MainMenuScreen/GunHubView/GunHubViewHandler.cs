using Cysharp.Threading.Tasks;
using Sources.DataBaseSystem;
using Sources.GamePlaySystem.MainMenuGame;
using Sources.GamePlaySystem.MainMenuGame.Store;
using Sources.Utils.Singleton;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace Game.Screens.MainMenuScreen
{
    public class GunHubViewHandler : MonoBehaviour
    {
        private StoreSystem _storeSystem => Locator<StoreSystem>.Instance;

        private TabState _tabState;

        [SerializeField] private GunHubView _gunHubView;

        public void OnSetUp(TabState tabState, IEnumerable<WeaponInfoBase> weaponInfos)
        {
            _tabState = tabState;

            foreach (var weaponInfo in weaponInfos)
            {
                var newHubView = Instantiate(_gunHubView);
                newHubView.transform.SetParent(transform);
                newHubView.OnSetUp(weaponInfo);
            }

            _storeSystem.TabCurrent.Subscribe(value =>
            {
                gameObject.SetActive(value == tabState);
            }).AddTo(this);
        }
    }
}