using System;
using System.Collections.Generic;
using UnityEngine;

namespace Sources.GameData
{
    public class WeaponModel
    {
        public string WeaponId;
        public string LevelUpgradeId;
    }

    public class StoreProfile : IProfileData
    {
        public WeaponModel LeaderWeapon;
        public WeaponModel BomberWeapon;

        public Dictionary<string, string> ShieldData = new();
        public string ShieldIdCurrent;

        public string GetLevelShieldCurrent()
        {
            return ShieldData[ShieldIdCurrent];
        }

        // For Test
        public void SetStoreDefault()
        {
            ShieldData.Add("shield-01", "level-01");
            ShieldIdCurrent = "shield-01";
        }
    }
}