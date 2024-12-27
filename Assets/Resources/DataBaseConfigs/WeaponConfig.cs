using Sirenix.OdinInspector;
using Sources.Language;
using Sources.Utils;
using System;
using System.Collections;
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

        private string GetDescription()
        {
            return Id;
        }
    }

    public abstract class WeaponInfoBase
    {
        public string Id;
        [PreviewField(100, ObjectFieldAlignment.Left)]
        public Sprite Icon;
        [PropertyOrder(1)]
        public int UnlockFee;

        [SerializeField, ValueDropdown(nameof(GetAllLanguageIds))]
        public string LanguageId;
        private IEnumerable GetAllLanguageIds => IdGetter.GetAllLanguageIds();

        [PropertyOrder(2), ListDrawerSettings(ListElementLabelName = "GetDescription")]
        public List<LevelUpgradeInfo> LevelUpgrades;
        protected Dictionary<string, LevelUpgradeInfo> LevelUpgradeCache { get; private set; } = new();
        [PropertyOrder(3)]
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
        [PropertyOrder(4)]
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
                var indexData = columnCount * rowCurrent + column;
                var newLevelUpgrade = new LevelUpgradeInfo
                {
                    Id = datas[indexData],
                    LevelUpFee = int.Parse(datas[indexData + columnCount]),
                    ReloadFee = int.Parse(datas[indexData + 2 * columnCount]),
                    DamageOrHp = int.Parse(datas[indexData + 3 * columnCount])
                };

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