using Cysharp.Threading.Tasks;
using Sources.DataBaseSystem;
using Sources.DataBaseSystem.Leader;
using Sources.Extension;
using Sources.GamePlaySystem.MainMenuGame;
using Sources.Utils.Singleton;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

namespace Game.Screens.MainMenuScreen
{
    public class GunHubView : MonoBehaviour
    {
        private StoreSystem _storeSystem => Locator<StoreSystem>.Instance;

        [SerializeField] private Image _icon;
        [SerializeField] private TMP_Text _textWeaponState;

        public void OnSetUp(WeaponInfoBase weaponInfoBase)
        {
            var storeWeaponHandler = _storeSystem.GetWeaponHandlerById(weaponInfoBase.Id);
            var weaponViewModel = storeWeaponHandler.WeaponWiewModels[weaponInfoBase.Id];

            _icon.sprite = weaponInfoBase.Icon;
            if (weaponInfoBase is LeaderWeaponInfo leaderWeaponInfo) SetSizeIconGun(leaderWeaponInfo);

            weaponViewModel.WeaponValue.Subscribe(value =>
            {
                _textWeaponState.text = value;
            }).AddTo(this);

            weaponViewModel.State.Subscribe(state =>
            {
                gameObject.SetActive(state == ItemState.AlreadyHave);
            }).AddTo(this);
        }

        private void SetSizeIconGun(LeaderWeaponInfo leaderWeaponInfo)
        {
            if (leaderWeaponInfo.Id == LeaderKey.GunId_03 || leaderWeaponInfo.Id == LeaderKey.GunId_04 || leaderWeaponInfo.Id == LeaderKey.GunId_05)
            {
                var rect = _icon.GetComponent<RectTransform>();
                Vector2 size = rect.sizeDelta;
                size.x = 110f;
                rect.sizeDelta = size;
            }
        }
    }
}