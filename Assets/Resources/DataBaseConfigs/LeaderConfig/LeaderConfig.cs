using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

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
        
        public List<DamageWeapon> DamageWeapons;
    }

    public class LeaderConfig : DataBaseConfig
    {
        [SerializeField] private List<WeaponInfo> _weaponInfo;
        public List<WeaponInfo > Weapons => _weaponInfo;
    }
}