using Sources.Utils.Singleton;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Sources.GameData
{
    public class GameData
    {
        //public UserData UserData = new();
        //public StoreData StoreData = new();

        //public void Init()
        //{
        //    Locator<GameData>.Set(this);
        //}

        private Dictionary<string, IProfileData> _profileDatasCache = new();

        public void SaveData(IProfileData profileData)
        {
            var filePath = Application.persistentDataPath + $"/{typeof(IProfileData).Name}.json";

            string jsonData = JsonUtility.ToJson(profileData);
            File.WriteAllText(filePath, jsonData);
        }

        public T GetProfileData<T>() where T : IProfileData
        {
            var nameFileProfile = typeof(T).Name;
            if (_profileDatasCache.ContainsKey(nameFileProfile)) 
                return (T)_profileDatasCache[nameFileProfile];

            DirectoryInfo directoryInfo = new DirectoryInfo(Application.persistentDataPath);
         
            foreach (FileInfo file in directoryInfo.GetFiles())
            {
                if (Path.GetFileNameWithoutExtension(file.Name) == typeof(T).Name)
                {
                    string json = File.ReadAllText(file.FullName);
                    T profileData = JsonUtility.FromJson<T>(json);

                    _profileDatasCache.Add(typeof(T).Name, profileData);

                    return profileData;
                }
            }

            throw new System.Exception($"Don't find profile data' name is: {typeof(T).Name}");
        }
    }
}