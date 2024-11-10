using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Sources.DataBaseSystem
{
    [Serializable]
    public class JourneyMapData
    {
        public int Rows;
        public int Collumns;
        // journey view id or link
        public List<string> Data_1 = new();
        // wave id or vertical/horizontal
        public List<string> Data_2 = new();
    }

    [Serializable]
    public class JourneyItemInfo
    {
        public string Id;
        public GameObject JourneyItem;
    }

    public class JourneyMapConfig : DataBaseConfig
    {
        [SerializeField] private List<JourneyItemInfo> _journeyItemViews;
        public List<JourneyItemInfo> JourneyItemViews => _journeyItemViews;
        private Dictionary<string, JourneyItemInfo> _journeyItemViewsCache = new();

        [SerializeField] private string _pathHolderJourneyItemPrefab;
        public string PathHolderJourneyItemPrefab => _pathHolderJourneyItemPrefab;

        public List<JourneyMapData> JourneyMapDatas = new();

        public JourneyItemInfo GetJourneyItemInfo(string id)
        {
            if (!_journeyItemViewsCache.ContainsKey(id))
            {
                var item = _journeyItemViews.Find(x  => x.Id == id);
                _journeyItemViewsCache.Add(id, item);
            }

            return _journeyItemViewsCache[id];
        }
    }
}