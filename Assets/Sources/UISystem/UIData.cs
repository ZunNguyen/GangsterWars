using UnityEngine;
using UnityEditor;
using UnityEngine.Windows;
using Sirenix.OdinInspector;
using UnityEditor.Callbacks;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using Sources.Utils.Singleton;

namespace Sources.UISystem
{
    public class UIData : ScriptableObject
    {
        private const string _defaultPathHolderUIData = "Assets/Resources/UIData";
        private const string _defaultPathHolderUIChildren = "Assets/Game/Screens";

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

        [Button]
        private void CreateNewUI(string uiName)
        {
            if(HaveNameUiInList(uiName)) return;
            CreateScript(uiName);
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

        private void CreateScript(string uiName)
        {
            if (uiName.Length == 0) return;
            if (uiName.Where(c => char.IsLetter(c)).Count() != uiName.Length) return;

            string pathHolder = $"{_defaultPathHolderUIChildren}/{uiName}";
            Directory.CreateDirectory(pathHolder);

            var go = PrefabUtility.LoadPrefabContents($"{_defaultPathHolderUIChildren}/{uiName}.prefab");
            PrefabUtility.SaveAsPrefabAsset(go, $"{_defaultPathHolderUIChildren}/{uiName}.prefab");
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