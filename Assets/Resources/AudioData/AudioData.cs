using Sirenix.OdinInspector;
using Sources.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Sources.DataBaseSystem
{
    [Serializable]
    public class AudioInfo
    {
        public string Id;
        public List<AudioClip> AudioClips = new();
        public bool IsMusic;

        public AudioClip TakeRandom()
        {
            return GetRandom.FromList(AudioClips);
        }

        private string GetDescription()
        {
            var music = IsMusic ? "Music" : "SFX";
            return $"[{Id}] / [{music}] / [{AudioClips.Count}]";
        }
    }

    [Serializable]
    public class AudioListInfo
    {
        public string Id;
        public List<AudioClip> AudioClips;

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

        [SerializeField, ListDrawerSettings(ListElementLabelName = "GetDescription")]
        private List<AudioListInfo> _audioListInfos;
        private Dictionary<string, AudioListInfo> _audioListInfoCache = new();

        public AudioInfo GetAudioInfo(string id)
        {
            if (!_audioInfoCache.ContainsKey(id))
            {
                var audioInfo = _audioInfos.Find(x => x.Id == id);
                _audioInfoCache.Add(id, audioInfo);
            }

            return _audioInfoCache[id];
        }

        public AudioListInfo GetAudioListInfo(string id)
        {
            if (!_audioListInfoCache.ContainsKey(id))
            {
                var audioListInfo = _audioListInfos.Find(x => x.Id == id);
                _audioListInfoCache.Add(id, audioListInfo);
            }

            return _audioListInfoCache[id];
        }
    }
}