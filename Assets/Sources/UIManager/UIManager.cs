using UnityEngine;
using UnityEditor;
using UnityEngine.Windows;
using Sirenix.OdinInspector;
using UnityEditor.Callbacks;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using Sources.Utils.Singleton;

namespace Sources.UIManager
{
    public class UIManager : ScriptableObject
    {
        private const string _defaultPathHolderUIManager = "Assets/Resources/UIManager";
        private const string _defaultPathHolderUIChildren = "Assets/Game/Screens";

        [SerializeField] private List<MonoBehaviour> _uis;
        [SerializeField] private List<string> _uiLayers;

        public static UIManager ActiveUIData
        {
            get
            {
                if (Locator<UIManager>.Instance == null)
                {
                    UIManager uiManager = AssetDatabase.FindAssets($"t: {nameof(UIManager)}").ToList()
                    .Select(AssetDatabase.GUIDToAssetPath)
                    .Select(AssetDatabase.LoadAssetAtPath<UIManager>)
                    .FirstOrDefault();

                    Locator<UIManager>.Set(uiManager);
                }

                return Locator<UIManager>.Instance;
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

            UIManager test = ScriptableObject.CreateInstance<UIManager>();
            AssetDatabase.CreateAsset(test, pathHolder);
        }

        [MenuItem("Assets/Create/Eren/UI/Create UI Manager")]
        private static void CreateUIManager()
        {
            UIManager uiManager = ScriptableObject.CreateInstance<UIManager>();

            string uiManagerPath = $"{_defaultPathHolderUIManager}/{typeof(UIManager).Name}.asset";
            
            if (AssetDatabase.LoadAssetAtPath<UIManager>(uiManagerPath))
            {
                Debug.LogWarning($"<color=red>{typeof(UIManager).Name}</color> aldready exist");
            }

            if (!Directory.Exists(_defaultPathHolderUIManager))
            {
                Directory.CreateDirectory(_defaultPathHolderUIManager);
            }

            AssetDatabase.CreateAsset(uiManager, uiManagerPath);
            AssetDatabase.SaveAssets();
        }
    }
}