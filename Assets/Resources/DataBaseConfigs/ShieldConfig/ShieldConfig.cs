using Sirenix.OdinInspector;
using Sources.GamePlaySystem.MainGamePlay;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Sources.DataBaseSystem
{
    [Serializable]
    public class ShieldInfo
    {
        public string Id;
        public List<IconInfo> Icons;
        public int UnlockFee;
        public List<LevelUpgradeInfo> LevelUpgrades = new List<LevelUpgradeInfo>();
        private Dictionary<string, LevelUpgradeInfo> _levelUpgradesCache = new(); 

        public Sprite GetIconShield(ShieldState shieldStates)
        {
            var iconInfo = Icons.Find(x => x.ShieldStates == shieldStates);
            return iconInfo.Icon;
        }

        public LevelUpgradeInfo GetLevelUpgradeInfo(string id)
        {
            if (!_levelUpgradesCache.ContainsKey(id))
            {
                var levelInfo = LevelUpgrades.Find(x => x.Id == id);
                _levelUpgradesCache.Add(id, levelInfo);
            }
            return _levelUpgradesCache[id];
        }

        public int GetLevelUpgradeIndex(string id)
        {
            var levelInfo = GetLevelUpgradeInfo(id);
            return LevelUpgrades.IndexOf(levelInfo);
        }
    }

    [Serializable]
    public class IconInfo
    {
        public ShieldState ShieldStates = ShieldState.Full;
        [PreviewField(100, ObjectFieldAlignment.Center)]
        public Sprite Icon;
    }

    [Serializable]
    public class LevelUpgradeShieldInfo
    {
        public string Id;
        public int LevelUpFee;
        public int ReloadFee;
        public int Hp;
    }

    public class ShieldConfig : DataBaseConfig
    {
        [SerializeField] private List<ShieldInfo> _shieldInfos;
        public List<ShieldInfo> ShieldInfos => _shieldInfos;

        private Dictionary<string, ShieldInfo> _shieldInfoCache = new();

        public ShieldInfo GetShieldInfo(string id)
        {
            if (!_shieldInfoCache.ContainsKey(id))
            {
                var shieldInfoTarget = _shieldInfos.Find(x => x.Id == id);
                _shieldInfoCache.Add(id, shieldInfoTarget);
            }

            return _shieldInfoCache[id];
        }

        public int GetShieldIndex(string id)
        {
            var shieldInfo = GetShieldInfo(id);
            return _shieldInfos.IndexOf(shieldInfo);
        }
    }
}