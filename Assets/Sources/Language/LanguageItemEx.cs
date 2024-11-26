using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sources.Language
{
    [Serializable]
    public class LanguageItemInfo
    {
        [HideInInspector]
        public string LanguageName;
        [LabelText("$LanguageName")][TextArea(5,10)]
        public string Text;
    }

    public class LanguageItemEx
    {

    }
}