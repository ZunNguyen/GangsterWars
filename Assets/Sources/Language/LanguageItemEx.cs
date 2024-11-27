using Sirenix.OdinInspector;
using Sources.Utils.Singleton;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    public interface ILanguageItem
    {
        public string Id { get;}
        public List<LanguageItemInfo> LanguageItemInfos { get;}
    }

    public static class LanguageItemEx
    {
        private static LanguageTable _languageTable => Locator<LanguageTable>.Instance;

        public static string GetText(this ILanguageItem languageItem)
        {
            if (languageItem == null) return string.Empty;

            var languageNameCurrent = _languageTable.LanguageDefault;

            var languageInfo = languageItem.LanguageItemInfos.Find(item => item.LanguageName == languageNameCurrent);
            return languageInfo.Text;
        }
    }
}