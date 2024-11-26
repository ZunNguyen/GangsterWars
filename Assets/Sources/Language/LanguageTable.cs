using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Sources.Language
{
    public class LanguageTable : ScriptableObject
    {
        [SerializeField] private List<string> _languages;

        [SerializeField] private List<LanguageItem> _languageItems;

        private const string _defaultConfigPath = "Assets/Resources/Language/LanguageItem";

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
    }
}