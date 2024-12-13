using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sources.FTUE.Config
{
    [Serializable]
    public class FTUKeyItem
    {
        public string Key;
        public List<string> Values;
    }

    [Serializable]
    public class FTUEKeyTable
    {
        [SerializeField]
        [ListDrawerSettings(ListElementLabelName = "Key")]
        private List<FTUKeyItem> _items;

        public List<string> GetAllKeyIds()
        {
            var ids = new List<string>();
            foreach (var item in _items)
            {
                foreach (var value in item.Values)
                {
                    ids.Add($"{item.Key}/{value}");
                }
            }
            return ids;
        }
    }
}