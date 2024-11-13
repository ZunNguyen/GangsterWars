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
        private const string _dataDefault = "";

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

        public int GetIndexWaveIdInJourneyMap(int indexGrid, string waveId)
        {
            var haveIdInData = JourneyMapDatas[indexGrid].Data_2.Contains(waveId);
            if (haveIdInData)
            {
                var indexData = JourneyMapDatas[indexGrid].Data_2.IndexOf(waveId);
                return indexData;
            }
            else return -1;
        }

        public bool HaveLinkIdWithIndex(int indexGrid, int indexLink)
        {
            if (indexLink < 0 || indexLink >= JourneyMapDatas[indexGrid].Data_1.Count) return false;

            var result = JourneyMapDatas[indexGrid].Data_1[indexLink];
            if (result == _dataDefault) return false;
            
            else return true;
        }

        public string GetWaveIdWithIndex(int indexGrid, int index)
        {
            var waveId = JourneyMapDatas[indexGrid].Data_2[index];
            return waveId;
        }
    }
}