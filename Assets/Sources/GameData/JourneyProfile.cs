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
    }
}