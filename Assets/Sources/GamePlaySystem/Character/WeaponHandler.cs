using Sources.DataBaseSystem;
using Sources.GameData;
using Sources.GamePlaySystem.MainGamePlay;
using Sources.Utils;
using Sources.Utils.Singleton;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;

namespace Sources.GamePlaySystem.Character
{
    public class WeaponView
    {
        public string WeaponId;
        public int Damage;
    }

    public class WeaponHandler
    {
        private GameData.GameData _gameData => Locator<GameData.GameData>.Instance;
        private UserProfile _userProfile => _gameData.GetProfileData<UserProfile>();
        private MainGamePlaySystem _mainGamePlaySystem => Locator<MainGamePlaySystem>.Instance;

        private bool _isReloading = false;
        private bool _isHaveEnemyToAttack = false;
        private bool _isAnimationComplete = true;
        private bool _isOutOfAmmor = false;

        private Dictionary<string, int> _damageWeaponCache = new();
        private List<WeaponData> _weaponDatasClone;
        private ReloadTimeHandler _reloadTimeHandler;
        private WeaponConfig _weaponConfig;

        public string WeaponIdCurrent {  get; private set; }
        public Action Attack;

        public void OnSetUp(List<WeaponData> weaponDatas, ReloadTimeHandler reloadTimeHandler, WeaponConfig weaponConfig)
        {
            _weaponDatasClone = new List<WeaponData>(weaponDatas);
            _reloadTimeHandler = reloadTimeHandler;
            _weaponConfig = weaponConfig;

            OnDestroy();
            _isOutOfAmmor = false;
            _reloadTimeHandler.IsReloading += SetCanAttack;
            _mainGamePlaySystem.SpawnEnemiesHandler.HaveEnemyToAttack += SetIsEnemyToAttack;
        }

        private void GetRandomWeapon()
        {
            if (_weaponDatasClone.Count == 0)
            {
                _isOutOfAmmor = true;
                return;
            }

            var model = GetRandom.FromList(_weaponDatasClone);
            if (model.Quatity != 0)
            {
                WeaponIdCurrent = model.Id;
                
                var weaponInfo = _weaponConfig.GetWeaponInfo(WeaponIdCurrent);
                var levelUpgradeInfo = weaponInfo.GetLevelUpgradeInfo(model.LevelUpgradeId);
                var damageWeapon = levelUpgradeInfo.DamageOrHp;
                if (!_damageWeaponCache.ContainsKey(model.Id))
                {
                    _damageWeaponCache.Add(WeaponIdCurrent, damageWeapon);
                }
                return;
            }
            else
            {
                _weaponDatasClone.Remove(model);
                GetRandomWeapon();
            }
        }

        public void SetCanAttack(bool status)
        {
            _isReloading = status;
            CheckCanAttack();
        }

        private void SetIsEnemyToAttack(bool status)
        {
            _isHaveEnemyToAttack = status;
            CheckCanAttack();
        }

        public void SetIsAnimationComplete(bool status)
        {
            _isAnimationComplete = status;
            CheckCanAttack();
        }

        private void CheckCanAttack()
        {
            if (!_isReloading && _isHaveEnemyToAttack && _isAnimationComplete)
            {
                GetRandomWeapon();
                if (_isOutOfAmmor) return;
                Attack?.Invoke();
            }
        }

        public void EndActionThrow()
        {
            _reloadTimeHandler.Reloading();
            _userProfile.SubsctractQualityWeapon(WeaponIdCurrent);
        }

        public int GetDamageWeapon()
        {
            return _damageWeaponCache[WeaponIdCurrent];
        }

        private void OnDestroy()
        {
            _reloadTimeHandler.IsReloading -= SetCanAttack;
            _mainGamePlaySystem.SpawnEnemiesHandler.HaveEnemyToAttack -= SetIsEnemyToAttack;
        }
    }
}