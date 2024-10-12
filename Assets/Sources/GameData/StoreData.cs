using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sources.GameData
{
    public class StoreData : IProfileData
    {
        public Dictionary<string, string> ShieldData = new();
        public string ShieldIdCurrent;

        public string GetLevelShieldCurrent()
        {
            return ShieldData[ShieldIdCurrent];
        }

        // For Test
        public StoreData()
        {
            ShieldData.Add("shield-01", "level-01");
            ShieldIdCurrent = "shield-01";
        }
    }
}