using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Sources.Language
{
    public class LanguageTable : ScriptableObject
    {
        [SerializeField][ReadOnly]
        private List<string> _languages;

        [SerializeField] private List<LanguageItem> _languageItems;
        private Dictionary<string, LanguageItem> _languageItemsCache = new();

        public IEnumerable<string> LanguageItemIds => _languageItems.Select(item => item.Id);

        [SerializeField]
        [ValueDropdown("ShowAllLanguage")]
        private string _languageDefault;
        public string LanguageDefault => _languageDefault;

        public Action OnChangeLanguageName;

        private List<string> ShowAllLanguage()
        {
            return _languages;
        }

        public LanguageItem GetLanguageItem(string id)
        {
            if (!_languageItemsCache.ContainsKey(id))
            {
                var languageItem = _languageItems.First(item => item.Id == id);
                _languageItemsCache.Add(id, languageItem);
            }

            return _languageItemsCache[id];
        }

        public void ChangeNexLanguageName()
        {
            if (_languages.Count == 1) return;

            int languageIndexCurrent = _languages.IndexOf(_languageDefault);
            int languageNextIndex = ++languageIndexCurrent % _languages.Count;

            while (_languages[languageNextIndex] == _languageDefault)
            {
                languageNextIndex = ++languageIndexCurrent % _languages.Count;
            }

            ChangeLanguageName(_languages[languageNextIndex]);
        }

        public void ChangeLanguageName(string languageName)
        {
            _languageDefault = languageName;
            OnChangeLanguageName?.Invoke();
        }

#if UNITY_EDITOR
        private static LanguageTable _instance;
        public static LanguageTable Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = AssetDatabase.FindAssets($"t: {typeof(LanguageTable).Name}").ToList()
                        .Select(AssetDatabase.GUIDToAssetPath)
                        .Select(AssetDatabase.LoadAssetAtPath<LanguageTable>)
                        .FirstOrDefault();
                }
                return _instance;
            }
        }

        private const string _defaultConfigPath = "Assets/Resources/Language/LanguageItem";

        [Header("------Tool------")]
        [SerializeField]
        [ValueDropdown("ShowAllLanguage")]
        private string _languageToDelete;

        [Button]
        public void RemoveLanguage()
        {
            if (!_languages.Contains(_languageToDelete))
            {
                Debug.LogError($"Language: {_languageToDelete} is aldready exits");
                return;
            }

            _languages.Remove(_languageToDelete);
            FecthAll();
            RemoveLanguageNameToLanguageItems(_languageToDelete);
            AssetDatabase.Refresh();
        }

        private void RemoveLanguageNameToLanguageItems(string languageName)
        {
            foreach (var languageItem in _languageItems)
            {
                var newLanguageInfo = languageItem.LanguageInfos.Find(x => x.LanguageName == languageName);
                languageItem.LanguageInfos.Remove(newLanguageInfo);
            }
        }

        [Button]
        public void CreatLanguage(string languageName)
        {
            if (_languages.Contains(languageName))
            {
                Debug.LogError($"Language: {languageName} is aldready exits");
                return;
            }

            _languages.Add(languageName);
            FecthAll();
            AddNewLanguageNameToLanguageItems(languageName);
            AssetDatabase.Refresh();
        }

        private void AddNewLanguageNameToLanguageItems(string languageName)
        {
            foreach (var languageItem in _languageItems)
            {
                var newLanguageInfo = new LanguageItemInfo();
                newLanguageInfo.LanguageName = languageName;
                languageItem.LanguageInfos.Add(newLanguageInfo);
            }
        }

        [Button]
        public void CreatLanguageItem(string id)
        {
            if (id == string.Empty) return;

            if (_languageItems.Find(x => x.Id == id) != null)
            {
                Debug.Log($"Language id: {id} is aldredy exits");
                return;
            }

            var asset = ScriptableObject.CreateInstance<LanguageItem>();
            AssetDatabase.CreateAsset(asset, $"{_defaultConfigPath}/{id}.asset");
            _languageItems.Add(asset);

            asset.Id = id;

            foreach (var languageName  in _languages)
            {
                var newLanguageName = new LanguageItemInfo();
                newLanguageName.LanguageName = languageName;
                asset.LanguageInfos.Add(newLanguageName);
            }

            EditorUtility.SetDirty(asset);
            AssetDatabase.Refresh();
        }

        [Button]
        public void FecthAll()
        {
            ClearNull();

            string[] guids = AssetDatabase.FindAssets("t:LanguageItem", new string[]{_defaultConfigPath});

            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                var asset = AssetDatabase.LoadAssetAtPath<LanguageItem>(path);

                if (!_languageItems.Contains(asset))
                {
                    _languageItems.Add(asset);
                }
            }
        }

        private void ClearNull()
        {
            _languageItems.RemoveAll(item => item == null);
        }
#endif
    }
}