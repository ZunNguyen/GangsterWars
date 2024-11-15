using Game.Character.Enemy;
using Sources.GameData;
using Sources.GamePlaySystem.MainGamePlay;
using Sources.Utils;
using Sources.Utils.Singleton;
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

        public ReactiveProperty<WeaponData> WeaponCurrent = new();
        public ReactiveProperty<EnemyController> EnemyTarget = new();

        public void OnSetUp(List<WeaponData> weaponDatas, ReloadTimeHandler reloadTimeHandler)
        {
            if (weaponDatas == null) return;

            _weaponDatas = weaponDatas;
            _reloadTimeHandler = reloadTimeHandler;
            reloadTimeHandler.CompleteReload += Start;
        }

        public void Start()
        {
            WeaponCurrent.Value = GetRandomWeapon();
            GetEnemyToAttack();
        }

        private WeaponData GetRandomWeapon()
        {
            var model = GetRandom.FromList(_weaponDatas);
            if (model.Quatity != 0)
            {
                return model;
            }
            return null;
        }

        private void GetEnemyToAttack()
        {
            if (_mainGamePlaySystem.SpawnEnemiesHandler.Enemies.Count == 0) return;
            EnemyTarget.Value = _mainGamePlaySystem.SpawnEnemiesHandler.Enemies[0];
        }

        public void EndActionThrow()
        {
            WeaponCurrent.Value = null;
            EnemyTarget.Value = null;
            _reloadTimeHandler.Reloading();
        }

        private void OnDestroy()
        {
            _reloadTimeHandler.CompleteReload -= Start;
        }
    }
}