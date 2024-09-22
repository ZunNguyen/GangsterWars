using Sirenix.OdinInspector;
using Sources.DataBaseSystem;
using Sources.GamePlaySystem.MainGamePlay;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Sources.DataBaseSystem
{
    [Serializable]
    public class ShieldInfo
    {
        public string Id;
        public List<IconInfo> Icons;
        public List<LevelInfo> Levels = new List<LevelInfo>();

        public LevelInfo GetLevelInfo(string id)
        {
            return Levels.Find(x => x.Id == id);
        }
    }

    [Serializable]
    public class LevelInfo
    {
        public string Id;
        public int Hp;
    }

    [Serializable]
    public class IconInfo
    {
        public ShieldState ShieldStates = ShieldState.Full;
        [PreviewField(100, ObjectFieldAlignment.Center)]
        public Sprite Icon;
    }

    public class ShieldConfig : DataBaseConfig
    {
        [SerializeField] private List<ShieldInfo> _shieldInfos;

        private Dictionary<string, ShieldInfo> _shieldInfoCache;

        public ShieldInfo GetShieldInfo(string id)
        {
            if (!_shieldInfoCache.ContainsKey(id))
            {
                var shieldInfoTarget = _shieldInfos.Find(x => x.Id == id);
                _shieldInfoCache.Add(id, shieldInfoTarget);
            }

            return _shieldInfoCache[id];
        }








        private void OnValidate()
        {
            foreach (var shieldInfo in _shieldInfos)
            {
                for (int i = 0; i < shieldInfo.Levels.Count; i++)
                {
                    var level = shieldInfo.Levels[i];
                    if (string.IsNullOrEmpty(level.Id))
                    {
                        // Automatically generate the Id based on shield and level index
                        level.Id = $"Level_{i + 1}";
                    }
                }
            }
        }
    }
}