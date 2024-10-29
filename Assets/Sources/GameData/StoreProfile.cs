using Sources.Extension;
using Sources.Utils;
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
        public List<WeaponData> BomberWeapons;

        public Dictionary<string, string> ShieldData = new();
        public string ShieldIdCurrent;

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