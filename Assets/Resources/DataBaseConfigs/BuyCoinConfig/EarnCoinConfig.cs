using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sources.DataBaseSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Sources.DataBaseSystem
{
    [Serializable]
    public class EarnCoinInfo
    {
        public string Id;
        [PreviewField(Alignment = ObjectFieldAlignment.Center, Height = 70)]
        public Sprite Sprite;
        public int Value;
        [Header("Minutes")]
        public int TimeToReload;
    }

    public class EarnCoinConfig : DataBaseConfig
    {
        [SerializeField] private List<EarnCoinInfo> _earnCoinInfos;
        private Dictionary<string, EarnCoinInfo> _earnCoinInfoCache = new();

        [SerializeField] private int _countCoinIcon;
        public int CountCoinIcon => _countCoinIcon;

        public EarnCoinInfo GetEarnCoinInfo(string id)
        {
            if (!_earnCoinInfoCache.ContainsKey(id))
            {
                var buyCoinInfo = _earnCoinInfos.Find(x => x.Id == id);
                _earnCoinInfoCache.Add(id, buyCoinInfo);
            }

            return _earnCoinInfoCache[id];
        }

        public List<EarnCoinInfo> GetAllInfos()
        {
            return _earnCoinInfos;
        }
    }
}