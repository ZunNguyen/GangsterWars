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
        public int MaxBullet;
        public int BulletsPerClip;

        [Header("Time to reload one bullet - second")]
        public float ReloadTime;

        private string GetDiscription()
        {
            return Id;
        }
    }

    public class BomberConfig : WeaponConfig
    {
        [SerializeField, ListDrawerSettings(ListElementLabelName = "GetDiscription")]
        private List<BomberWeaponInfo> _bomberWeaponInfos;
        Dictionary<string, BomberWeaponInfo> _bomberWeaponInfoCache = new();

        [SerializeField] private int _openFee;
        public int OpenFee => _openFee;

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