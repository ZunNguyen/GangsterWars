using Sources.DataBaseSystem;
using Sources.DataBaseSystem.Leader;
using Sources.GamePlaySystem.MainMenuGame;
using Sources.GamePlaySystem.MainMenuGame.Store;
using Sources.SpawnerSystem;
using Sources.Utils.Singleton;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Screens.MainMenuScreen
{
    public class WeaponView : MonoBehaviour
    {
        private StoreSystem _storeSystem => Locator<StoreSystem>.Instance;
        private SpawnerManager _spawnerManager => Locator<SpawnerManager>.Instance;

        private WeaponInfoBase _weaponInfo;
        private Sources.GamePlaySystem.MainMenuGame.Store.WeaponViewModel _weaponViewModel;
        private StoreHandlerBase _storeWeaponHandler;
        private ShieldStoreHandler _shieldStoreHandler;
        private string _weaponId;
        

        [Header("Level Upgrade")]
        [SerializeField] private LevelUpgradeView _levelUpgradePrefab;
        [SerializeField] private Transform _levelUpgradeHolder;

        [Header("Level Up")]
        [SerializeField] private GameObject _levelUpFee;
        [SerializeField] private TMP_Text _valueLevelUpFee;

        [Header("Reload")]
        [SerializeField] private GameObject _reload;
        [SerializeField] private TMP_Text _valueReload;

        [Header("Unlock")]
        [SerializeField] private GameObject _unlock;
        [SerializeField] private TMP_Text _valueUnlock;

        [Header("Check List")]
        [SerializeField] private GameObject _checkList;
        [SerializeField] private GameObject _iconCheckList;

        [Header("Another")]
        [SerializeField] private Image _icon;
        [SerializeField] private GameObject _iconlock;

        private void Awake()
        {
            _checkList.SetActive(false);
            _iconCheckList.SetActive(false);
        }

        public void OnSetUp(WeaponInfoBase weaponInfo)
        {
            _weaponId = weaponInfo.Id;
            _storeWeaponHandler = _storeSystem.GetWeaponHandlerSystem(_weaponId);
            _weaponViewModel = _storeWeaponHandler.WeaponWiewModels[_weaponId];
            
            _weaponInfo = weaponInfo;
            if (weaponInfo is LeaderWeaponInfo leaderWeaponInfo)
            {
                _icon.sprite = leaderWeaponInfo.Icon;
            }
            else if (weaponInfo is BomberWeaponInfo bomberWeaponInfo)
            {
                _icon.sprite = bomberWeaponInfo.Icon;
            }
            else if (weaponInfo is ShieldWeaponInfo shieldWeaponInfo)
            {
                _icon.sprite = shieldWeaponInfo.Icon;
            }

            GetWeaponSate();
            GetLevelUpgrade();
            GetReloadFee();
            GetUnlockFee();
        }

        private void GetWeaponSate()
        {
            _weaponViewModel.State.Subscribe(state =>
            {
                if (state == ItemState.AlreadyHave)
                {
                    _iconlock.SetActive(false);
                    _unlock.SetActive(false);

                    _levelUpgradeHolder.gameObject.SetActive(true);
                    _levelUpFee.SetActive(true);

                    GetOnCheckList();
                }
                if (state == ItemState.CanUnlock)
                {
                    _iconlock.SetActive(true);
                    _unlock.SetActive(true);

                    _levelUpgradeHolder.gameObject.SetActive(false);
                    _levelUpFee.SetActive(false);
                }
                if (state == ItemState.CanNotUnlock)
                {
                    _iconlock.SetActive(true);

                    _unlock.SetActive(false);
                    _levelUpgradeHolder.gameObject.SetActive(false);
                    _levelUpFee.SetActive(false);
                }

            }).AddTo(this);
        }

        private void GetLevelUpgrade()
        {
            _weaponViewModel.LevelUpgradeFee.Subscribe(value =>
            {
                if(value == 0)
                {
                    _levelUpFee.SetActive(false);
                }
            }).AddTo(this);

            _valueLevelUpFee.text = _weaponViewModel.LevelUpgradeFee.ToString();

            for (int i = 0; i < _weaponInfo.LevelUpgrades.Count; i++)
            {
                var newLevelUpgrade = Instantiate(_levelUpgradePrefab, _levelUpgradeHolder);
                newLevelUpgrade.OnSetUp(_weaponInfo.LevelUpgrades[i].Id, _weaponViewModel);
            }
        }

        private void GetReloadFee()
        {
            _weaponViewModel.ReloadFee.Subscribe(value =>
            {
                _reload.SetActive(value != 0);
                _valueReload.text = value.ToString();
            }).AddTo(this);
        }

        private void GetUnlockFee()
        {
            _valueUnlock.text = _weaponViewModel.UnlockFee.ToString();
        }

        private void GetOnCheckList()
        {
            if (_storeWeaponHandler is ShieldStoreHandler shieldStoreHandler)
            {
                _shieldStoreHandler = shieldStoreHandler;
                _checkList.SetActive(true);
                _weaponViewModel.IsChosed += SubscribeCheckList;

                OnCheckListClicked();
            }
        }

        private void SubscribeCheckList(bool isCheck)
        {
            if (isCheck) _iconCheckList.SetActive(true);
            else _iconCheckList.SetActive(false);
        }

        public void OnCheckListClicked()
        {
            _shieldStoreHandler.ChoseShield(_weaponId);
        }

        public void OnUnlockWeaponClicked()
        {
            _storeWeaponHandler.UnlockNewWeapon(_weaponId);
        }

        public void OnLevelUpgradeClicked()
        {
            _storeWeaponHandler.UpgradeNewLevelWeapon(_weaponId);
        }

        public void OnReloadClicked()
        {
            _storeWeaponHandler.ReloadWeapon(_weaponId);
        }
    }
}