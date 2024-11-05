using Game.Character.Bomber;
using Resources.CSV;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.U2D.Animation;

namespace Sources.DataBaseSystem.Leader
{
    [Serializable]
    public class LevelUpgradeInfo
    {
        public string Id;
        public int LevelUpFee;
        public int ReloadFee;
        public int Damage;
    }

    [Serializable]
    public class WeaponInfo : IReadCSVData 
    {
        public string Id;
        
        [PreviewField(100, ObjectFieldAlignment.Left)]
        public Sprite Icon;
        public SpriteLibraryAsset SpriteLibraryAsset;
        public int UnlockFee;
        public int MaxBullet;
        public int BulletsPerClip;

        [Header("Time to reload one bullet - second")] 
        public float ReloadTime;
        
        public List<LevelUpgradeInfo> LevelUpgrades;

        public Dictionary<string, LevelUpgradeInfo> LevelUpgradeCache { get; private set; } = new();
        

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

    public class LeaderConfig : WeaponConfig
    {
        
    }
}