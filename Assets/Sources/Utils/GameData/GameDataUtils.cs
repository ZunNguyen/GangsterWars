using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Sources.Utils
{
    public static class GameDataUtils
    {
        [MenuItem("Tools/Clear Game Data")]
        public static void ClearAllData()
        {
            var isClear = EditorUtility.DisplayDialog("Clear All Data?", "All data will be clear. Do you want it?", "Yes", "No");
            if (isClear)
            {
                ClearData();
                UnityEngine.Debug.Log("Done");
            }
        }

        public static void ClearData()
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(Application.persistentDataPath);
         
            foreach (FileInfo fileInfo in directoryInfo.GetFiles())
            {
                fileInfo.Delete();
            }

            foreach (DirectoryInfo dir in directoryInfo.GetDirectories())
            { 
                dir.Delete();
            }
        }


        [MenuItem("Tools/Open Persistant Folder")]
        public static void PersistantFolder()
        {
            Process.Start(Application.persistentDataPath);
        }
    }
}