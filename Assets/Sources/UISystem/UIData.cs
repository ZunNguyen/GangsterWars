using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector;
using UnityEditor.Callbacks;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using Sources.Utils.Singleton;
using Sources.UI;
using System.IO;
using Sources.DataBaseSystem;
using UnityEditor.Compilation;
using Cysharp.Threading.Tasks;
using System;

namespace Sources.UISystem
{
    public class UIData : ScriptableObject
    {
        private const string _defaultKeySavePlayerPrefs = "NewUI";

        private const string _defaultPathHolderUIData = "Assets/Resources/UI/UIData";
        private const string _defaultPathHolderUIChildren = "Assets/Game/Screens";
        private const string _pathHolderUITemplatePrefab = "Assets/Resources/UI/UITemplate/UITemplate.prefab";
        private const string _pathHolderUITemplateScript = "Assets/Sources/UISystem/UITemplate/UITemplate.txt";
        private const string _nameSpace = "Game.Screens";

        [SerializeField] private List<MonoBehaviour> _uis;
        [SerializeField] private List<string> _uiLayers;

        public static UIData ActiveUIData
        {
            get
            {
                if (Locator<UIData>.Instance == null)
                {
                    UIData uiManager = AssetDatabase.FindAssets($"t: {nameof(UIData)}").ToList()
                    .Select(AssetDatabase.GUIDToAssetPath)
                    .Select(AssetDatabase.LoadAssetAtPath<UIData>)
                    .FirstOrDefault();

                    Locator<UIData>.Set(uiManager);
                }

                return Locator<UIData>.Instance;
            }
        }

        public List<string> GetUILayers()
        {
            if (_uiLayers.Count == 0) _uiLayers.Add("Default");
            return _uiLayers;
        }
        
        [SerializeField] private List<BaseUI> uis = new List<BaseUI>();

        [Button]
        private void CreateNewUI(string uiName)
        {
            if(HaveNameUiInList(uiName)) return;
            CreateScriptAndPrefab(uiName);
        }

        private bool HaveNameUiInList(string uiName)
        {
            foreach (var ui in _uis)
            {
                if (ui.name == uiName)
                {
                    Debug.LogWarning($"<color=red>{uiName}</color> had aldready exist");
                    return true;
                }
            }
            return false;
        }

        private void CreateScriptAndPrefab(string uiName)
        {
            if (uiName.Length == 0) return;
            if (uiName.Where(c => char.IsLetter(c)).Count() != uiName.Length) return;

            string pathHolderFolder = $"{_defaultPathHolderUIChildren}/{uiName}";
            var pathHolderPrefab = $"{pathHolderFolder}/{uiName}.prefab";
            var pathHolderScript = $"{pathHolderFolder}/{uiName}.cs";

            Directory.CreateDirectory(pathHolderFolder);

            string dataUITemplateScript = File.ReadAllText(_pathHolderUITemplateScript);
            dataUITemplateScript = dataUITemplateScript.Replace("@ScriptName", uiName);
            dataUITemplateScript = dataUITemplateScript.Replace("@NameSpace", $"{_nameSpace}.{uiName}");

            using (StreamWriter writer = new StreamWriter(pathHolderScript))
            {
                writer.Write(dataUITemplateScript);
            }
            AssetDatabase.ImportAsset(pathHolderScript);
            AssetDatabase.Refresh();
            
            AssetDatabase.CopyAsset(_pathHolderUITemplatePrefab, pathHolderPrefab);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            PlayerPrefs.SetString(_defaultKeySavePlayerPrefs, uiName);
        }

        [DidReloadScripts]
        private static void OnReloadScriptFinish()
        {
            string uiName = PlayerPrefs.GetString(_defaultKeySavePlayerPrefs);
            PlayerPrefs.DeleteKey(_defaultKeySavePlayerPrefs);

            string pathHolderFolder = $"{_defaultPathHolderUIChildren}/{uiName}";
            var pathHolderPrefab = $"{pathHolderFolder}/{uiName}.prefab";
            var pathHolderScript = $"{pathHolderFolder}/{uiName}.cs";

            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(pathHolderPrefab);
            var script = AssetDatabase.LoadAssetAtPath<MonoScript>(pathHolderScript);

            var scriptType = script.GetClass();
            prefab.AddComponent(scriptType);
            PrefabUtility.SavePrefabAsset(prefab);
        }

        [MenuItem("Assets/Create/Eren/UI/Create UI Data")]
        private static void CreateUIManager()
        {
            UIData uiData = ScriptableObject.CreateInstance<UIData>();
            
            string uiDataPath = $"{_defaultPathHolderUIData}/{typeof(UIData).Name}.asset";
            
            if (AssetDatabase.LoadAssetAtPath<UIData>(uiDataPath))
            {
                Debug.LogWarning($"<color=red>{typeof(UIData).Name}</color> aldready exist");
            }

            if (!Directory.Exists(_defaultPathHolderUIData))
            {
                Directory.CreateDirectory(_defaultPathHolderUIData);
            }

            AssetDatabase.CreateAsset(uiData, uiDataPath);
            AssetDatabase.SaveAssets();
        }
    }
}