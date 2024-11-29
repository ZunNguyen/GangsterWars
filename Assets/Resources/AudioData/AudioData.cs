using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Sources.DataBaseSystem
{
    [Serializable]
    public class AudioInfo
    {
        public string Id;
        public AudioClip AudioClip;
        public bool IsMusic;

        private string GetDescription()
        {
            return Id;
        }
    }

    public class AudioData : ScriptableObject
    {
        [SerializeField, ListDrawerSettings(ListElementLabelName = "GetDescription")]
        private List<AudioInfo> _audioInfos;
        private Dictionary<string, AudioInfo> _audioInfoCache = new();

        public AudioInfo GetAudioInfo(string id)
        {
            if (!_audioInfoCache.ContainsKey(id))
            {
                var audioInfo = _audioInfos.Find(x => x.Id == id);
                _audioInfoCache.Add(id, audioInfo);
            }

            return _audioInfoCache[id];
        }
    }
}