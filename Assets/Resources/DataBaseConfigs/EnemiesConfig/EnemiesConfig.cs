using Sirenix.OdinInspector;
using Sources.DataBaseSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Sources.DataBaseSystem
{
    [Serializable]
    public class EnemyInfo
    {
        public string Id;
        [PreviewField(100, ObjectFieldAlignment.Center)]
        public GameObject Enemy;
    }

    public class EnemiesConfig : DataBaseConfig
    {
        public List<EnemyInfo> Enemies;
    }
}