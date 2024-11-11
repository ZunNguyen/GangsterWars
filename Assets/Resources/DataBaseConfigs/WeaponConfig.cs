using Resources.CSV;
using Sirenix.OdinInspector;
using Sources.DataBaseSystem.Leader;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Sources.DataBaseSystem
{
    [Serializable]
    public class LevelUpgradeInfo
    {
        public string Id;
        public int LevelUpFee;
        public int ReloadFee;
        [Header("Damage or Hp")]
        public int DamageOrHp;
    }

    public abstract class WeaponInfoBase : IReadCSVData
    {
        public string Id;
        [PreviewField(100, ObjectFieldAlignment.Left)]
        public Sprite Icon;
        [PropertyOrder(1)]
        public int UnlockFee;

        [PropertyOrder(2)]
        public List<LevelUpgradeInfo> LevelUpgrades;
        protected Dictionary<string, LevelUpgradeInfo> LevelUpgradeCache { get; private set; } = new();
        public TextAsset CSVFile;

        public LevelUpgradeInfo GetLevelUpgradeInfo(string id)
        {
            if (!LevelUpgradeCache.ContainsKey(id))
            {
                var damageWeapon = LevelUpgrades.Find(x => x.Id == id);
                LevelUpgradeCache.Add(id, damageWeapon);
                return damageWeapon;
            }

            return LevelUpgradeCache[id];
        }

        public int GetLevelUpgardeIndex(string levelUpgradeId)
        {
            var levelUpgradeInfo = LevelUpgrades.FirstOrDefault(level => level.Id == levelUpgradeId);
            return LevelUpgrades.IndexOf(levelUpgradeInfo);
        }

#if UNITY_EDITOR
        [PropertyOrder(3)]
        [Button]
        public void ReadFile()
        {
            LevelUpgrades.Clear();
            string[] datas = CSVFile.text.Split(new string[] { ",", "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries);
            string[] lines = CSVFile.text.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);

            var rowCount = lines.Length;
            var columnCount = datas.Length / rowCount;
            var rowCurrent = 1;

            for (int column = 1; column < columnCount; column++)
            {
                var newLevelUpgrade = new LevelUpgradeInfo();
                var indexData = columnCount * rowCurrent + column;

                newLevelUpgrade.Id = datas[indexData];
                newLevelUpgrade.LevelUpFee = int.Parse(datas[indexData + columnCount]);
                newLevelUpgrade.ReloadFee = int.Parse(datas[indexData + 2 * columnCount]);

                LevelUpgrades.Add(newLevelUpgrade);
            }
        }
#endif
    }

    public abstract class WeaponConfig : DataBaseConfig
    {
        public abstract IEnumerable<WeaponInfoBase> GetAllWeapons();

        public abstract WeaponInfoBase GetWeaponInfo(string id);

        public abstract int GetWeaponIndex(string id);
    }
}