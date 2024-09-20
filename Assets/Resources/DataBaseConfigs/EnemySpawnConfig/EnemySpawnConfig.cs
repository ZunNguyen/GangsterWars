using BestHTTP.SecureProtocol.Org.BouncyCastle.Bcpg.Sig;
using Sources.DataBaseSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Sources.DataBaseSystem
{
    [Serializable]
    public class Wave
    {
        public string Id;
        public List<Turn> Turns;
    }

    [Serializable]
    public class Turn
    {
        public string Id;
        public List<Phase> Phases;
    }

    [Serializable]
    public class Phase
    {
        public string PhaseId;
        public Enemy Enemy;
        public int SpawnAfterMiliSeccond;
    }

    [Serializable]
    public class Enemy
    {
        public string EnemyId;
        public List<int> IndexPos;
        public int Damage;
        public int HP;
    }

    public class EnemySpawnConfig : DataBaseConfig
    {
        [SerializeField] private List<Wave> _waves;
        public Dictionary<string, Wave> WavesCache;

        public Wave GetWaveInfo(string id)
        {
            if (!WavesCache.ContainsKey(id))
            {
                var wave = _waves.Find(waveTarget => waveTarget.Id == id);
                WavesCache.Add(id, wave);
            }

            return WavesCache[id];
        }
    }
}