using Sources.DataBaseSystem.Leader;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Sources.DataBaseSystem
{
    public class WeaponConfig : DataBaseConfig
    {
        [SerializeField] private List<WeaponInfo> _weapons;
        public List<WeaponInfo> Weapons => _weapons;

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

        public int GetWeaponIndex(string weaponId)
        {
            var weaponInfo = _weapons.FirstOrDefault(weapon => weapon.Id == weaponId);
            return _weapons.IndexOf(weaponInfo);
        }
    }
}