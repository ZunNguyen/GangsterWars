using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;

namespace Sources.DataBaseSystem.Leader
{
    [Serializable]
    public class DamageWeapon
    {
        public string Level;
        public int Damage;
    }

    [Serializable]
    public class WeaponInfo
    {
        public string Id;
        
        [PreviewField(100, ObjectFieldAlignment.Left)]
        public Sprite Icon;

        public SpriteLibraryAsset SpriteLibraryAsset;
        public int MaxBullet;
        public int BulletsPerClip;

        [Header("Time to reload one bullet - millisecond")] 
        public int ReloadTime;
        
        public List<DamageWeapon> DamageWeapons;

        public Dictionary<string, DamageWeapon> DamageWeaponCache { get; private set; }

        public DamageWeapon GetDamageWeapon(string id)
        {
            if (!DamageWeaponCache.ContainsKey(id))
            {
                var damageWeapon = DamageWeapons.Find(x => x.Level == id);
                DamageWeaponCache.Add(id, damageWeapon);
                return damageWeapon;
            }

            return DamageWeaponCache[id];
        }
    }

    public class LeaderConfig : DataBaseConfig
    {
        [SerializeField] private List<WeaponInfo> _weapons;
        public List<WeaponInfo > Weapons => _weapons;

        public Dictionary<string, WeaponInfo> WeaponInfoCache { get; private set; } = new Dictionary<string, WeaponInfo>();

        public WeaponInfo GetWeaponInfo(string id)
        {
            if (!WeaponInfoCache.ContainsKey(id))
            {
                var weaponInfo = _weapons.Find(x => x.Id == id);
                WeaponInfoCache.Add(id, weaponInfo);
                return weaponInfo;
            }

            return WeaponInfoCache[id];
        }
    }
}