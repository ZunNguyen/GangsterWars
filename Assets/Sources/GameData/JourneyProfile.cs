using Sources.Extension;
using Sources.GameData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sources.GameData
{
    public class WaveData
    {
        public string Id;
        public int Stars;
    }

    public class JourneyProfile : IProfileData
    {
        public List<WaveData> WavesPassedDatas = new();
        private Dictionary<string, WaveData> _waveDataCache = new();
 
        public void SetJourneyDataDefault()
        {
            var newWaveData = new WaveData
            {
                Id = JourneyKey.WaveDefault,
                Stars = 0,
            };
            WavesPassedDatas.Add(newWaveData);
            Save();
        }

        public WaveData GetWaveData(string id)
        {
            if (!_waveDataCache.ContainsKey(id))
            {
                var waveData = WavesPassedDatas.Find(x => x.Id == id);
                _waveDataCache.Add(id, waveData);
            }

            return _waveDataCache[id];
        }
    }
}