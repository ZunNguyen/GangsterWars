using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;

namespace Sources.GameData
{
    public class GameData
    {
        private Dictionary<string, IProfileData> _profileDatasCache = new();

        public void SaveData(IProfileData profileData)
        {
            var fileName = profileData.GetType().Name;

            string jsonData = JsonConvert.SerializeObject(profileData, Formatting.Indented);

#if UNITY_WEBGL
            PlayerPrefs.SetString(fileName, jsonData);
            PlayerPrefs.Save();
#else
            var filePath = Application.persistentDataPath + $"/{fileName}.json";
            File.WriteAllText(filePath, jsonData);
#endif
        }

        public T GetProfileData<T>() where T : IProfileData, new()
        {
            var nameFileProfile = typeof(T).Name;

            if (_profileDatasCache.ContainsKey(nameFileProfile))
                return (T)_profileDatasCache[nameFileProfile];

#if UNITY_WEBGL
            if (PlayerPrefs.HasKey(nameFileProfile))
            {
                string json = PlayerPrefs.GetString(nameFileProfile);
                T profileData = JsonConvert.DeserializeObject<T>(json);

                _profileDatasCache.Add(nameFileProfile, profileData);
                return profileData;
            }
#else
            DirectoryInfo directoryInfo = new DirectoryInfo(Application.persistentDataPath);

            foreach (FileInfo file in directoryInfo.GetFiles())
            {
                if (Path.GetFileNameWithoutExtension(file.Name) == nameFileProfile)
                {
                    string json = File.ReadAllText(file.FullName);
                    T profileData = JsonConvert.DeserializeObject<T>(json);

                    _profileDatasCache.Add(nameFileProfile, profileData);
                    return profileData;
                }
            }
#endif

            var newProfileData = new T();
            _profileDatasCache.Add(nameFileProfile, newProfileData);
            SaveData(newProfileData);
            return newProfileData;
        }
    }
}