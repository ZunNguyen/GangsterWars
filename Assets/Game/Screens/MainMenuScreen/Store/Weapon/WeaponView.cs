using Sirenix.OdinInspector.Editor.Validation;
using Sources.DataBaseSystem;
using Sources.GameData;
using Sources.GamePlaySystem.MainMenuGame;
using Sources.SpawnerSystem;
using Sources.Utils.Singleton;
using System.Collections.Generic;
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

        public ReactiveProperty<List<string>> _levelsUpgardeAvailable { get; private set; } 
            = new ReactiveProperty<List<string>>(new List<string>());

        [Header("Level Upgrade")]
        [SerializeField] private LevelUpgradeView _levelUpgradePrefab;
        [SerializeField] private Transform _levelUpgradeHolder;

        [SerializeField] private Image _icon;
        [SerializeField] private GameObject _unlock;
        [SerializeField] private GameObject _iconlock;
        [SerializeField] private GameObject _levelUp;
        [SerializeField] private GameObject _reload;

        public void OnSetUp(WeaponData weaponData)
        {
            _weaponInfo = _storeConfig.GetWeaponInfo(weaponData.WeaponId);

            _icon.sprite = _weaponInfo.Icon;

            var weaponState = _storeSystem.StoreLeaderWeaponHandler.GetWeaponState(weaponData.WeaponId);
            if (weaponState == WeaponState.AlreadyHave)
            {
                _unlock.SetActive(false);
                _iconlock.SetActive(false);
                GetLevelUpgrade(weaponData.LevelUpgradeId);
            }
            if (weaponState == WeaponState.CanUnlock || weaponState == WeaponState.CanNotUnlock)
            {
                _levelUp.SetActive(false);
                _reload.SetActive(false);
            }
        }

        private void GetLevelUpgrade(string levelUpgradeId)
        {
            var levelUpgradeIndex = _weaponInfo.GetIndexLevelUpgrade(levelUpgradeId);

            for (int i = 0; i < _weaponInfo.LevelUpgrades.Count; i++)
            {
                var newLevelUpgrade = Instantiate(_levelUpgradePrefab, _levelUpgradeHolder);

                if (levelUpgradeIndex <= i) _levelsUpgardeAvailable.Value.Add(_weaponInfo.LevelUpgrades[i].Id);

                newLevelUpgrade.OnSetUp(_weaponInfo.LevelUpgrades[i].Id, _levelsUpgardeAvailable);
            }
        }
    }
}