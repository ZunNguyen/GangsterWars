using Sirenix.OdinInspector;
using Sources.DataBaseSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Sources.DataBaseSystem
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
        public List<DamageWeapon> DamageWeapon;
    }

    public class LeaderConfig : DataBaseConfig
    {
        [SerializeField] private List<WeaponInfo> _weaponInfo;
    }
}