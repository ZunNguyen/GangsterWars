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
    public class GunInfo
    {
        public string Id;
        
        [PreviewField(100, ObjectFieldAlignment.Left)]
        public Sprite Icon;

        public SpriteLibraryAsset SpriteLibraryAsset;
        public int MaxBullet;
        public int BulletsPerClip;

        [Header("Time to reload one bullet - second")] 
        public float ReloadTime;
        
        public List<DamageWeapon> DamageWeapons;

        public Dictionary<string, DamageWeapon> DamageWeaponCache { get; private set; } = new();

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
        [SerializeField] private List<GunInfo> _weapons;
        public List<GunInfo > Weapons => _weapons;

        public Dictionary<string, GunInfo> WeaponInfoCache { get; private set; } = new Dictionary<string, GunInfo>();

        public GunInfo GetWeaponInfo(string id)
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