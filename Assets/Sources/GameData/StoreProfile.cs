using Sources.DataBaseSystem;
using Sources.Extension;
using Sources.Utils;
using Sources.Utils.String;
using System.Collections.Generic;

namespace Sources.GameData
{
    public class WeaponData
    {
        public string WeaponId;
        public string LevelUpgradeId;
    }

    public class StoreProfile : IProfileData
    {
        public List<WeaponData> LeaderWeapons;
        private Dictionary<string, WeaponData> _leaderWeaponCache = new();

        public List<WeaponData> BomberWeapons;
        private Dictionary<string, WeaponData> _bomberWeaponCache = new();

        public Dictionary<string, string> ShieldData = new();
        public string ShieldIdCurrent;

        public WeaponData GetWeaponInfo(string weaponId)
        {
            var weapons = GetCorrectListWeapon(weaponId).weapons;
            var weaponCache = GetCorrectListWeapon(weaponId).weaponCache;

            if (!weaponCache.ContainsKey(weaponId))
            {
                var weapon = weapons.Find(x => x.WeaponId == weaponId);
                weaponCache.Add(weaponId, weapon);
            }

            return weaponCache[weaponId];
        }

        private (List<WeaponData> weapons, Dictionary<string, WeaponData> weaponCache) GetCorrectListWeapon(string weaponId)
        {
            var baseWeaponId = StringUtils.GetBaseName(weaponId);

            var baseWeaponLeaderId = StringUtils.GetBaseName(LeaderWeapons[0].WeaponId);
            if (baseWeaponId == baseWeaponLeaderId)
            {
                return (LeaderWeapons, _leaderWeaponCache);
            }

            var baseWeaponBomberId = StringUtils.GetBaseName(BomberWeapons[0].WeaponId);
            if (baseWeaponId == baseWeaponBomberId)
            {
                return (BomberWeapons, _bomberWeaponCache);
            }

            return (null, null);
        }

        public string GetLevelShieldCurrent()
        {
            return ShieldData[ShieldIdCurrent];
        }

        public void SetStoreShieldDefault()
        {
            ShieldData.Add("shield-01", "level-01");
            ShieldIdCurrent = "shield-01";

            Save();
        }

        public void SetStoreLeaderDefault()
        {
            LeaderWeapons = new();
            var weaponDefault = new WeaponData();
            weaponDefault.WeaponId = LeaderKey.GunId_01;
            weaponDefault.LevelUpgradeId = LeaderKey.Level_01;
            LeaderWeapons.Add(weaponDefault);
            
            Save();
        }

        public void SetStoreBomberDefault()
        {
            BomberWeapons = new();
            var weaponDefault = new WeaponData();
            weaponDefault.WeaponId = BomberKey.BomberId_Default;
            weaponDefault.LevelUpgradeId = BomberKey.Level_Default;
            BomberWeapons.Add(weaponDefault);

            Save();
        }
    }
}