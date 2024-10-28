using Sirenix.OdinInspector;
using Sources.Utils.String;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Sources.DataBaseSystem
{
    [Serializable]
    public class WeaponInfo
    {
        public string Id;
        [PreviewField(100, ObjectFieldAlignment.Left)]
        public Sprite Icon;
        public int UnlockFee;
        public List<LevelUpgradeInfo> LevelUpgrades;
    }

    [Serializable]
    public class LevelUpgradeInfo
    {
        public string Id;
        public int LevelUpFee;
        public int ReloadFee;
    }

    public class StoreConfig : DataBaseConfig
    {
        [TabGroup("LeaderStore")]
        [SerializeField] private List<WeaponInfo> _leaderWeapons;
        public List<WeaponInfo> LeaderWeapons => _leaderWeapons;
        private Dictionary<string, WeaponInfo> _leaderWeaponCache = new();

        [TabGroup("BomberStore")]
        [SerializeField] private List<WeaponInfo> _bomberWeapons;
        public List<WeaponInfo> BomberWeapons => _bomberWeapons;
        private Dictionary<string, WeaponInfo> _bomberWeaponCache = new();

        public WeaponInfo GetWeaponInfo(string weaponId)
        {
            var weapons = GetCorrectListWeapon(weaponId).weapons;
            var weaponCache = GetCorrectListWeapon(weaponId).weaponCache;

            if (!weaponCache.ContainsKey(weaponId))
            {
                var weapon = weapons.Find(x => x.Id == weaponId);
                weaponCache.Add(weaponId, weapon);
            }

            return weaponCache[weaponId];
        }

        private (List<WeaponInfo> weapons, Dictionary<string, WeaponInfo> weaponCache) GetCorrectListWeapon(string weaponId)
        {
            var baseWeaponId = StringUtils.GetBaseName(weaponId);

            var baseWeaponLeaderId = StringUtils.GetBaseName(_leaderWeapons[0].Id);
            if (baseWeaponId == baseWeaponLeaderId)
            {
                return (_leaderWeapons, _leaderWeaponCache);
            }

            var baseWeaponBomberId = StringUtils.GetBaseName(_bomberWeapons[0].Id);
            if (baseWeaponId == baseWeaponBomberId)
            {
                return (_bomberWeapons, _bomberWeaponCache);
            }

            return (null, null);
        }
    }
}