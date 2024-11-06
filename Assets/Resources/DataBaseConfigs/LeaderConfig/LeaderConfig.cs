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
    public class LeaderWeaponInfo : WeaponInfoBase
    {
        [PreviewField(100, ObjectFieldAlignment.Left)]
        public Sprite Icon;
        public SpriteLibraryAsset SpriteLibraryAsset;
        public int MaxBullet;
        public int BulletsPerClip;

        [Header("Time to reload one bullet - second")] 
        public float ReloadTime;
        
        protected override void SetValue(string[] datas, string[] lines)
        {
            var rowCount = lines.Length;

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

    public class LeaderConfig : WeaponConfig
    {
        [SerializeField]
        private List<LeaderWeaponInfo> _weaponInfos;
        private Dictionary<string, LeaderWeaponInfo> _weaponInfoCache = new();

        public override IEnumerable<WeaponInfoBase> GetAllWeapons()
        {
            return _weaponInfos;
        }

        public override WeaponInfoBase GetWeaponInfo(string id)
        {
            if (!_weaponInfoCache.ContainsKey(id))
            {
                var weaponInfo = _weaponInfos.Find(x => x.Id == id);
                _weaponInfoCache.Add(id, weaponInfo);
            }

            return _weaponInfoCache[id];
        }

        public override int GetWeaponIndex(string id)
        {
            var weaponInfo = GetWeaponInfo(id) as LeaderWeaponInfo;
            return _weaponInfos.IndexOf(weaponInfo);
        }
    }
}