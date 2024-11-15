using Game.Character.Enemy;
using Sources.GameData;
using Sources.GamePlaySystem.Leader;
using Sources.GamePlaySystem.MainGamePlay;
using Sources.Utils;
using Sources.Utils.Singleton;
using System;
using System.Collections.Generic;
using UniRx;

namespace Sources.GamePlaySystem.Character
{
    public class WeaponView
    {
        public string WeaponId;
        public int Damage;
    }

    public class WeaponHandler
    {
        private MainGamePlaySystem _mainGamePlaySystem => Locator<MainGamePlaySystem>.Instance;

        private List<WeaponData> _weaponDatas;
        private ReloadTimeHandler _reloadTimeHandler;
        private bool _isReloading = false;
        private bool _isHaveEnemyToAttack = false;
        private bool _isAnimationComplete = true;

        public ReactiveProperty<WeaponData> WeaponCurrent = new();
        public Action Attack;

        public void OnSetUp(List<WeaponData> weaponDatas, ReloadTimeHandler reloadTimeHandler)
        {
            if (weaponDatas == null) return;

            _weaponDatas = weaponDatas;
            _reloadTimeHandler = reloadTimeHandler;
            _reloadTimeHandler.IsReloading += SetCanAttack;
            _mainGamePlaySystem.SpawnEnemiesHandler.HaveEnemyToAttack += SetIsEnemyToAttack;
        }

        private void GetRandomWeapon()
        {
            var model = GetRandom.FromList(_weaponDatas);
            if (model.Quatity != 0)
            {
                WeaponCurrent.Value = model;
                Attack?.Invoke();
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
                Attack?.Invoke();
            }
        }

        public void EndActionThrow()
        {
            WeaponCurrent.Value = null;
            _reloadTimeHandler.Reloading();
        }

        private void OnDestroy()
        {
            _reloadTimeHandler.IsReloading -= SetCanAttack;
            _mainGamePlaySystem.SpawnEnemiesHandler.HaveEnemyToAttack -= SetIsEnemyToAttack;
        }
    }
}