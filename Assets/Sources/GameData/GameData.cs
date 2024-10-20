using Newtonsoft.Json;
using Sources.GameData;
using Sources.Utils.Singleton;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
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
            var fileName = profileData.GetType().Name;

            var filePath = Application.persistentDataPath + $"/{fileName}.json";

            string jsonData = JsonConvert.SerializeObject(profileData, Formatting.Indented);
            File.WriteAllText(filePath, jsonData);
        }

        public T GetProfileData<T>() where T : IProfileData , new()
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
                    T profileData = JsonConvert.DeserializeObject<T>(json);

                    _profileDatasCache.Add(typeof(T).Name, profileData);

                    return profileData;
                }
            }

            var newProfileData = new T();
            _profileDatasCache.Add(typeof(T).Name, newProfileData);
            newProfileData.Save();
            return newProfileData;
        }
    }
}