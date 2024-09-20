using Sirenix.OdinInspector;
using Sources.DataBaseSystem;
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
        [PreviewField(100, ObjectFieldAlignment.Center)]
        public Sprite Icon;
        public List<Level> Levels = new List<Level>();
    }

    [Serializable]
    public class Level
    {
        public string Id;
        public int Hp;
    }

    public class ShieldConfig : DataBaseConfig
    {
        public List<ShieldInfo> ShieldInfos;

        private void OnValidate()
        {
            foreach (var shieldInfo in ShieldInfos)
            {
                for (int i = 0; i < shieldInfo.Levels.Count; i++)
                {
                    var level = shieldInfo.Levels[i];
                    if (string.IsNullOrEmpty(level.Id))
                    {
                        // Automatically generate the Id based on shield and level index
                        level.Id = $"{shieldInfo.Id}_Level_{i + 1}";
                    }
                }
            }
        }
    }
}