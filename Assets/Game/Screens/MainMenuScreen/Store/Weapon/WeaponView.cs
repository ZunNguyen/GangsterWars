using Sources.DataBaseSystem;
using Sources.GameData;
using Sources.GamePlaySystem.CoinController;
using Sources.GamePlaySystem.MainMenuGame;
using Sources.SpawnerSystem;
using Sources.Utils.Singleton;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Screens.MainMenuScreen
{
    public class WeaponView : MonoBehaviour
    {
        private DataBase _dataBase => Locator<DataBase>.Instance;
        private StoreConfig _storeConfig => _dataBase.GetConfig<StoreConfig>();

        private StoreSystem _storeSystem => Locator<StoreSystem>.Instance;
        private SpawnerManager _spawnerManager => Locator<SpawnerManager>.Instance;

        private WeaponInfo _weaponInfo;
        private WeaponViewModel _weaponViewModel;
        private Sources.GamePlaySystem.MainMenuGame.StoreWeaponHandler _storeWeaponHandler;
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

        [Header("Another")]
        [SerializeField] private Image _icon;
        [SerializeField] private GameObject _iconlock;

        public void OnSetUp(string weaponId)
        {
            _weaponId = weaponId;
            _storeWeaponHandler = _storeSystem.GetWeaponHandlerSystem(weaponId);
            _weaponViewModel = _storeWeaponHandler.WeaponWiewModels[weaponId];
            
            _weaponInfo = _storeConfig.GetWeaponInfo(weaponId);
            _icon.sprite = _weaponInfo.Icon;
            
            GetWeaponSate();
            GetLevelUpgrade();
            GetReloadFee();
            GetUnlockFee();
        }

        private void GetWeaponSate()
        {
            _weaponViewModel.State.Subscribe(state =>
            {
                if (state == WeaponState.AlreadyHave)
                {
                    _iconlock.SetActive(false);
                    _unlock.SetActive(false);

                    _levelUpgradeHolder.gameObject.SetActive(true);
                    _levelUpFee.SetActive(true);
                    _reload.SetActive(true);
                }
                if (state == WeaponState.CanUnlock)
                {
                    _iconlock.SetActive(true);
                    _unlock.SetActive(true);

                    _levelUpgradeHolder.gameObject.SetActive(false);
                    _levelUpFee.SetActive(false);
                    _reload.SetActive(false);
                }
                if (state == WeaponState.CanNotUnlock)
                {
                    _iconlock.SetActive(true);

                    _unlock.SetActive(false);
                    _levelUpgradeHolder.gameObject.SetActive(false);
                    _levelUpFee.SetActive(false);
                    _reload.SetActive(false);
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

        private void GetUnlockFee()
        {
            _valueUnlock.text = _weaponViewModel.UnlockFee.ToString();
        }

        private void GetReloadFee()
        {
            _valueReload.text = _weaponViewModel.ReloadFee.ToString();
        }

        public void OnUnlockWeaponClicked()
        {
            _storeWeaponHandler.UnlockNewWeapon(_weaponId);
        }

        public void OnLevelUpgradeClicked()
        {
            _storeWeaponHandler.UpgradeNewLevelWeapon(_weaponId);
        }
    }
}