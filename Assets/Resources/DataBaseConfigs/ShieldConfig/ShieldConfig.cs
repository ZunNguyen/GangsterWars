﻿using Sirenix.OdinInspector;
using Sources.GamePlaySystem.MainGamePlay;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Sources.DataBaseSystem
{
    [Serializable]
    public class ShieldWeaponInfo : WeaponInfoBase
    {
        public List<IconStateInfo> IconStates;

        public Sprite GetIconShield(ShieldState shieldStates)
        {
            var iconInfo = IconStates.Find(x => x.ShieldStates == shieldStates);
            return iconInfo.Icon;
        }

        private string GetDescription()
        {
            return Id;
        }
    }

    [Serializable]
    public class IconStateInfo
    {
        public ShieldState ShieldStates = ShieldState.Full;
        [PreviewField(100, ObjectFieldAlignment.Center)]
        public Sprite Icon;
    }

    public class ShieldConfig : WeaponConfig
    {
        [SerializeField, ListDrawerSettings(ListElementLabelName = "GetDescription")]
        private List<ShieldWeaponInfo> _shieldInfos;
        private Dictionary<string, ShieldWeaponInfo> _shieldInfoCache = new();

        public override IEnumerable<WeaponInfoBase> GetAllWeapons()
        {
            return _shieldInfos;
        }

        public override WeaponInfoBase GetWeaponInfo(string id)
        {
            if (!_shieldInfoCache.ContainsKey(id))
            {
                var shieldInfoTarget = _shieldInfos.Find(x => x.Id == id);
                _shieldInfoCache.Add(id, shieldInfoTarget);
            }

            return _shieldInfoCache[id];
        }

        public override int GetWeaponIndex(string id)
        {
            var shieldInfo = GetWeaponInfo(id) as ShieldWeaponInfo;
            return _shieldInfos.IndexOf(shieldInfo);
        }
    }
}