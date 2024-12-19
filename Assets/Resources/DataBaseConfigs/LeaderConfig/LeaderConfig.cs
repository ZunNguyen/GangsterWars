using Game.Character.Bomber;
using Resources.CSV;
using Sirenix.OdinInspector;
using Sources.Language;
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
        public SpriteLibraryAsset SpriteLibraryAsset;
        public int MaxBullet;
        public int BulletsPerClip;
        public bool InfinityBullet;

        [Header("Time to reload one bullet - second")] 
        public float ReloadTime;

        private string GetDescription()
        {
            return Id;
        }
    }

    public class LeaderConfig : WeaponConfig
    {
        [SerializeField, ListDrawerSettings(ListElementLabelName = "GetDescription")]
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