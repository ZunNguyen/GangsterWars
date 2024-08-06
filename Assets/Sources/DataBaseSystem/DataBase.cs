using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector;
using System.Linq;
using System;
using System.IO;
using UnityEditor.Compilation;

namespace Sources.DataBaseSystem
{
    public class DataBase : ScriptableObject
    {
        private const string _defaultConfigPath = "Assets/Resources/DataBaseConfigs";

        [Button]
        private void CreateNewDataBaseConfig(string scriptName)
        {
            if (scriptName.Length == 0) return;
            if (scriptName.Where(c => Char.IsLetter(c)).Count() != scriptName.Length) return;

            string folderPatrh = $"{_defaultConfigPath}/{scriptName}";
            string scriptPath = $"{folderPatrh}/{scriptName}.cs";
            if (!Directory.Exists(_defaultConfigPath)) Directory.CreateDirectory(_defaultConfigPath);

            if (Directory.Exists(folderPatrh))
            {
                Debug.LogError($"DataBase Config with name <color=red>{scriptName}</color> aldready exist");
                return;
            }

            Directory.CreateDirectory(folderPatrh);

            string templatePath = Path.Combine(Application.dataPath, "Sources/DataBaseSystem/DataBaseConfigGenerator/DataBaseConfigTemplate.txt");
            string dataTemplateContent = File.ReadAllText(templatePath);
            dataTemplateContent = dataTemplateContent.Replace("@ScriptName", scriptName);
            dataTemplateContent = dataTemplateContent.Replace("@NameSpace", typeof(DataBase).Namespace);
            // do not overwrite
            using (StreamWriter outFile = File.CreateText(scriptPath))
            {
                outFile.Write(dataTemplateContent);
            }
            AssetDatabase.Refresh();
            if (PlayerPrefs.HasKey("NewConfig"))
            {
                PlayerPrefs.SetString("NewConfig", scriptName);
            }
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
}