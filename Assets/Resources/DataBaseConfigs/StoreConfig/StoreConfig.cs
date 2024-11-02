using Resources.CSV;
using Sirenix.OdinInspector;
using Sources.Utils.String;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Sources.DataBaseSystem
{
    [Serializable]
    public class WeaponInfo : IReadCSVData
    {
        public string Id;
        [PreviewField(100, ObjectFieldAlignment.Left)]
        public Sprite Icon;
        public int UnlockFee;
        public List<LevelUpgradeInfo> LevelUpgrades;
        private Dictionary<string, LevelUpgradeInfo> _levelUpgradeCache = new();
    
        public int GetIndexLevelUpgrade(string id)
        {
            var levelUpgrade = LevelUpgrades.Find(x => x.Id == id);
            return LevelUpgrades.IndexOf(levelUpgrade);
        }

        public LevelUpgradeInfo GetLevelUpgradeInfo(string id)
        {
            if (!_levelUpgradeCache.ContainsKey(id))
            {
                var levelUpgrade = LevelUpgrades.Find(x => x.Id == id);
                _levelUpgradeCache.Add(id, levelUpgrade);
            }
            return _levelUpgradeCache[id];
        }

        [Button]
        public void ReadFile(TextAsset csvFile)
        {
            LevelUpgrades.Clear();
            string[] datas = csvFile.text.Split(new string[] { ",", "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries);
            
            string[] lines = csvFile.text.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
            // 4
            var rowCount = lines.Length;
            // 11
            var columnCount = datas.Length / rowCount;

            for (int column = 1; column < columnCount; column++)
            {
                var rowCurrent = 1;
                var newLevelUpgrade = new LevelUpgradeInfo();
                var indexData = columnCount * rowCurrent + column;

                newLevelUpgrade.Id = datas[indexData];
                newLevelUpgrade.LevelUpFee = int.Parse(datas[indexData + columnCount]);
                newLevelUpgrade.ReloadFee = int.Parse(datas[indexData + 2 * columnCount]);

                LevelUpgrades.Add(newLevelUpgrade);
            }
        }
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
        [TabGroup("LeaderStore", TabName = "LeaderStore")]
        [SerializeField] private List<WeaponInfo> _leaderWeapons;
        public List<WeaponInfo> LeaderWeapons => _leaderWeapons;
        private Dictionary<string, WeaponInfo> _leaderWeaponCache = new();

        [TabGroup("BomberStore", TabName = "BomberStore")]
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