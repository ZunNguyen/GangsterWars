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
        public int Damage;
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

        [PropertyOrder(3)]
        [Button]
        public void ReadFile(TextAsset csvFile)
        {
            LevelUpgrades.Clear();
            string[] datas = csvFile.text.Split(new string[] { ",", "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries);

            string[] lines = csvFile.text.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);

            SetValue(datas, lines);
        }

        protected abstract void SetValue(string[] datas, string[] lines);

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
    }

    public abstract class WeaponConfig : DataBaseConfig
    {
        public abstract IEnumerable<WeaponInfoBase> GetAllWeapons();

        public abstract WeaponInfoBase GetWeaponInfo(string id);

        public abstract int GetWeaponIndex(string id);
    }
}