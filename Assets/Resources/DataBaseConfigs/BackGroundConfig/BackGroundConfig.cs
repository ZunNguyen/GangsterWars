using Sirenix.OdinInspector;
using Sources.DataBaseSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using UnityEngine;

namespace Sources.DataBaseSystem
{
    [Serializable]
    public class WaveInfo
    {
        public string WaveId;
        public Sprite Sprite;
    }

    public class BackGroundConfig : DataBaseConfig
    {
        [SerializeField] private List<WaveInfo> Waves = new();
        private Dictionary<string, WaveInfo> _wavesCache = new();

        public WaveInfo GetWaveInfo(string waveId)
        {
            if (!_wavesCache.ContainsKey(waveId))
            {
                var waveInfo = Waves.Find(x => x.WaveId == waveId);
                _wavesCache.Add(waveId, waveInfo);
            }

            return _wavesCache[waveId];
        }

#if UNITY_EDITOR
        [Button]
        public void Creat()
        {
            Clear();

            string waveNum = "";
            for (int i = 1; i <= 40; i++)
            {
                var wave = new WaveInfo();
                if (i < 10)
                {
                    waveNum = "0" + i.ToString();
                }
                else
                {
                    waveNum = i.ToString();
                }

                wave.WaveId = $"wave-{waveNum}";
                Waves.Add(wave);
            }
        }

        [Button]
        public void Clear()
        {
            Waves.Clear();
        }

        [Button]
        public void AddSprite(Sprite sprite, int indexBegin, int indexFinish)
        {
            for (int i = indexBegin - 1; i < indexFinish; i++)
            {
                Waves[i].Sprite = sprite;
            }
        }
#endif
    }
}