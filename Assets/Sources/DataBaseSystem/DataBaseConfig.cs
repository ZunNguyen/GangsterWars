using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sources.DataBaseSystem
{
    public class DataBaseConfig : ScriptableObject
    {
        public string ID => GetType().Name;
    }
}