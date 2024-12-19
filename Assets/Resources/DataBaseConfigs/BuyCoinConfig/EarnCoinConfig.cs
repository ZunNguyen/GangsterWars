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
    public class BuyCoinInfo
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
        [SerializeField] private List<BuyCoinInfo> _buyCoinInfos;
        private Dictionary<string, BuyCoinInfo> _buyCoinInfoCache = new();

        [SerializeField] private int _countCoinIcon;
        public int CountCoinIcon => _countCoinIcon;

        public BuyCoinInfo GetBuyCoinInfo(string id)
        {
            if (!_buyCoinInfoCache.ContainsKey(id))
            {
                var buyCoinInfo = _buyCoinInfos.Find(x => x.Id == id);
                _buyCoinInfoCache.Add(id, buyCoinInfo);
            }

            return _buyCoinInfoCache[id];
        }

        public List<BuyCoinInfo> GetAllInfos()
        {
            return _buyCoinInfos;
        }
    }
}