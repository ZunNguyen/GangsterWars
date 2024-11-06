using Sirenix.OdinInspector;
using Sirenix.Utilities;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;

namespace Sources.DataBaseSystem
{
    [Serializable]
    public class BomberWeaponInfo : WeaponInfoBase
    {
        [PreviewField(100, ObjectFieldAlignment.Left)]
        public Sprite Icon;
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

    public class BomberConfig : WeaponConfig
    {
        [SerializeField]
        private List<BomberWeaponInfo> _bomberWeaponInfos;
        Dictionary<string, BomberWeaponInfo> _bomberWeaponInfoCache = new();

        public override IEnumerable<WeaponInfoBase> GetAllWeapons()
        {
            return _bomberWeaponInfos;
        }

        public override WeaponInfoBase GetWeaponInfo(string id)
        {
            if (!_bomberWeaponInfoCache.ContainsKey(id))
            {
                var weaponInfo = _bomberWeaponInfos.Find(x => x.Id == id);
                _bomberWeaponInfoCache.Add(id, weaponInfo);
            }

            return _bomberWeaponInfoCache[id];
        }

        public override int GetWeaponIndex(string id)
        {
            var weaponInfo = GetWeaponInfo(id) as BomberWeaponInfo;
            return _bomberWeaponInfos.IndexOf(weaponInfo);
        }
    }
}