using Sirenix.OdinInspector;
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

        [TabGroup("BomberStore")]
        [SerializeField] private List<WeaponInfo> _bomberWeapons;
    }
}