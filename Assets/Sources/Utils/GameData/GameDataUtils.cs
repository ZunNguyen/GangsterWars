using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Sources.Utils
{
    public static class GameDataUtils
    {
#if UNITY_EDITOR
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
#endif

        public static void ClearData()
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(Application.persistentDataPath);
         
            foreach (FileInfo fileInfo in directoryInfo.GetFiles())
            {
                fileInfo.Delete();
            }

            foreach (DirectoryInfo dir in directoryInfo.GetDirectories())
            {
                try
                {
                    dir.Delete(true);
                }
                catch {}
            }
        }

#if UNITY_EDITOR

        [MenuItem("Tools/Open Persistant Folder")]
        public static void PersistantFolder()
        {
            Process.Start(Application.persistentDataPath);
        }
#endif
    }
}