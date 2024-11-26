using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sources.Language
{
    public class LanguageItem : ScriptableObject
    {
        [ReadOnly] public string Id;
        public List<LanguageItemInfo> LanguageInfos;
    }
}