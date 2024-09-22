using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sources.GameData
{
    public class StoreData
    {
        public Dictionary<string, string> ShieldData;
        public string ShieldIdCurrent;

        public string GetLevelShieldCurrent()
        {
            return ShieldData[ShieldIdCurrent];
        }
    }
}