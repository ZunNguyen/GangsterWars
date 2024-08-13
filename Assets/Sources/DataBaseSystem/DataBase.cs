using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector;
using System.Linq;
using System;
using System.IO;
using UnityEditor.Compilation;
using UnityEditor.Callbacks;
using System.Collections.Generic;

namespace Sources.DataBaseSystem
{
    public class DataBase : ScriptableObject
    {
        private const string _defaultConfigPath = "Assets/Resources/DataBaseConfigs";
        private static DataBase _editorInstance;
        [InlineEditor]
        [SerializeField] private List<DataBaseConfig> _configs;

        public static DataBase EditorInstance
        {
            get
            {
                if (_editorInstance == null) _editorInstance = Resources.Load<DataBase>(nameof(DataBase));

                return _editorInstance;
            }
        }

        public void Add(DataBaseConfig config)
        {
            ClearNull();

            if (_configs.Contains(config))
            {
                Debug.LogError($"DBConfig: {config.ID} already exists!");
                return;
            }
            else
            {
                _configs.Add(config);
            }
        }

        public void ClearNull()
        {
            _configs.RemoveAll(item => item == null);
        }

#if UNITY_EDITOR
        [Button]
        private void CreateNewDataBaseConfig(string scriptName)
        {
            if (scriptName.Length == 0) return;
            if (scriptName.Where(c => Char.IsLetter(c)).Count() != scriptName.Length) return;

            string folderPath = $"{_defaultConfigPath}/{scriptName}";
            string scriptPath = $"{folderPath}/{scriptName}.cs";
            if (!Directory.Exists(_defaultConfigPath)) Directory.CreateDirectory(_defaultConfigPath);

            if (Directory.Exists(folderPath))
            {
                Debug.LogError($"DataBase Config with name <color=red>{scriptName}</color> aldready exist");
                return;
            }

            Directory.CreateDirectory(folderPath);

            string templatePath = Path.Combine(Application.dataPath, "Sources/DataBaseSystem/DataBaseConfigTemplate/DataBaseConfigTemplate.txt");
            string dataTemplateContent = File.ReadAllText(templatePath);
            dataTemplateContent = dataTemplateContent.Replace("@ScriptName", scriptName);
            dataTemplateContent = dataTemplateContent.Replace("@NameSpace", typeof(DataBase).Namespace);
            // do not overwrite
            using (StreamWriter outFile = File.CreateText(scriptPath))
            {
                outFile.Write(dataTemplateContent);
            }
            AssetDatabase.Refresh();
            PlayerPrefs.SetString("NewConfig", scriptName);
            CompileScript();
        }

        private void CompileScript()
        {
            AssetDatabase.Refresh();
            EditorUtility.RequestScriptReload();
            CompilationPipeline.RequestScriptCompilation();
            CompilationPipeline.assemblyCompilationFinished += OnCompileFinish;
        }

        private void OnCompileFinish(string arg1, CompilerMessage[] arg2)
        {
            AssetDatabase.Refresh();
            EditorUtility.RequestScriptReload();
            CompilationPipeline.assemblyCompilationFinished -= OnCompileFinish;
        }

        [DidReloadScripts]
        private static void OnReloadScriptFinish()
        {
            string scriptName = PlayerPrefs.GetString("NewConfig", "");
            PlayerPrefs.DeleteKey("NewConfig");
            if (scriptName.Length == 0) return;
            CreateScriptableObject(scriptName);
        }

        private static void CreateScriptableObject(string scriptName)
        {
            Type type = System.Type.GetType($"{typeof(DataBaseConfig).Namespace}.{scriptName}, Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null");
            if (type == null)
            {
                return;
            }

            DataBaseConfig asset = (DataBaseConfig)ScriptableObject.CreateInstance(type);
            AssetDatabase.CreateAsset(asset, $"{_defaultConfigPath}/{scriptName}/{type.Name}.asset");
            DataBase.EditorInstance.Add(asset);
            AssetDatabase.SaveAssets();
        }

        [MenuItem("Assets/Create/Eren/DataBase/Create DataBase System")]
        private static void CreateDataBaseSystem()
        {
            DataBase dataBase = ScriptableObject.CreateInstance<DataBase>();
            if (!Directory.Exists(_defaultConfigPath))
            { 
                Directory.CreateDirectory(_defaultConfigPath);
            }

            AssetDatabase.CreateAsset(dataBase, "Assets/Resources/DataBase.asset");
            AssetDatabase.SaveAssets();
        }
    }
#endif
}