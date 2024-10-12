using Sirenix.OdinInspector;
using Sources.DataBaseSystem;
using Sources.DataBaseSystem.Leader;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.U2D.Animation;

namespace Sources.DataBaseSystem
{
    [Serializable]
    public class BomInfo
    {
        public string Id;

        [PreviewField(100, ObjectFieldAlignment.Left)]
        public Sprite Icon;

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

    public class BomberConfig : DataBaseConfig
    {
        [SerializeField] private List<BomInfo> _weapons;
        public List<BomInfo> Weapons => _weapons;

        [Header("Milisecond")]
        [SerializeField] private float _reloadingTime;
        public float ReloadingTime => _reloadingTime;

        public Dictionary<string, BomInfo> WeaponInfoCache { get; private set; } = new Dictionary<string, BomInfo>();

        public BomInfo GetWeaponInfo(string id)
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