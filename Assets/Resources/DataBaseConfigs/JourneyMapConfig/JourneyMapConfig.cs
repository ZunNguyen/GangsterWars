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
        public List<string> Data_1;
        public List<string> Data_2;
    }

    [Serializable]
    public class JourneyItemView
    {
        public string Id;
        public GameObject JourneyItem;
    }

    public class JourneyMapConfig : DataBaseConfig
    {
        public List<JourneyItemView> JourneyItemViews = new();
        public List<JourneyMapData> JourneyMapDatas = new();
    }
}