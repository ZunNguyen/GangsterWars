using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UniRx;
using UnityEditor;
using UnityEngine;

namespace Sources.Language
{
    public class LanguageTable : ScriptableObject
    {
        [SerializeField][ReadOnly]
        private List<string> _languages;

        [SerializeField, InlineEditor] private List<LanguageItem> _languageItems = new();
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
                if (languageItem == null) return null;
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

#if UNITY_EDITOR
        private const string _defaultConfigPath = "Assets/Resources/Language/LanguageItem";

        [PropertySpace(20)]
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

        [PropertySpace(30)]
        [SerializeField, PropertyOrder(1), ReadOnly] private TextAsset _csvFile;
        [SerializeField, PropertyOrder(2), ReadOnly] private string _csvFilePath;
        [PropertyOrder(3)]
        public void ReadFile()
        {
            FecthAll();

            string[] datas = _csvFile.text.Split(new string[] {",", "\n", "\r"}, StringSplitOptions.RemoveEmptyEntries);
            string[] lines = _csvFile.text.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);

            var rowCount = lines.Length;
            var colCount = datas.Length / rowCount;

            List<LanguageItem> languageItemsTemp = new();
            LanguageItem languageItem = null;
            int indexLanguageId;
            string languageId;

            LanguageItemInfo languageItemInfo;
            string dataLanguageVietNam;
            string dataLanguageEnglish;

            for (int row = 1; row < rowCount; row++)
            {
                indexLanguageId = row * colCount;
                languageId = datas[indexLanguageId];

                if (languageItemsTemp.FirstOrDefault(x => x.Id == languageId) != null) continue;

                languageItem = GetLanguageItem();
                UpdateData();
            }

            foreach (var languageItemRemain in _languageItems)
            {
                var path = AssetDatabase.GetAssetPath(languageItem);
                AssetDatabase.DeleteAsset(path);
            }

            _languageItems.Clear();
            _languageItems = languageItemsTemp;
            FecthAll();
            AssetDatabase.Refresh();

            void UpdateData()
            {
                languageItemsTemp.Add(languageItem);
                _languageItems.Remove(languageItem);

                dataLanguageVietNam = datas[indexLanguageId + 1];
                languageItemInfo = languageItem.LanguageItemInfos[0];
                languageItemInfo.Text = dataLanguageVietNam;

                dataLanguageEnglish = datas[indexLanguageId + 2];
                languageItemInfo = languageItem.LanguageItemInfos[1];
                languageItemInfo.Text = dataLanguageEnglish;
            }

            LanguageItem GetLanguageItem()
            {
                languageItem = _languageItems.FirstOrDefault(item => item.Id == languageId);
                if (languageItem == null)
                {
                    CreatLanguageItem(languageId);
                    languageItem = _languageItems.FirstOrDefault(item => item.Id == languageId);
                }

                return languageItem;
            }
        }

        public void UpdateToFile(LanguageItem languageItem)
        {
            string[] datas = _csvFile.text.Split(new string[] { ",", "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries);
            string[] lines = _csvFile.text.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);

            var rowCount = lines.Length;
            var colCount = datas.Length / rowCount;

            string newLine = "";

            var indexLanguage = Array.IndexOf(datas, languageItem.Id);   
            if (indexLanguage == -1)
            {
                newLine = $"{languageItem.Id},{languageItem.LanguageItemInfos[0].Text},{languageItem.LanguageItemInfos[1].Text}";
            }
            else
            {
                datas[indexLanguage + 1] = languageItem.LanguageInfos[0].Text; // Viet namese text
                datas[indexLanguage + 2] = languageItem.LanguageInfos[1].Text; // English text
            }

            List<string> linesData = new ();
            for (int i = 0; i < rowCount; i++)
            {
                var lineData = new List<string>();
                for (int j = 0; j < colCount; j++)
                {
                    lineData.Add(datas[i * colCount + j]);
                }
                linesData.Add(string.Join(",", lineData));
            }

            if (newLine != "")
            {
                linesData.Add(newLine);
            }

            File.WriteAllLines(_csvFilePath, linesData);
        }
#endif
    }
}