using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sources.Language
{
    public class LanguageItem : ScriptableObject, ILanguageItem
    {
        [ReadOnly] public string Id;
        public List<LanguageItemInfo> LanguageInfos = new();

        string ILanguageItem.Id 
        {   
            get => Id; 
        }

        public List<LanguageItemInfo> LanguageItemInfos
        {
            get => LanguageInfos;
        }
    }
}