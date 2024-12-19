using Cysharp.Threading.Tasks;
using Sources.SystemService;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Sources.TimeManager
{
    public class InitTimeManagerSystemService : InitSystemService<TimeManagerSystem> { }

    public class TimeManagerSystem : BaseSystem
    {
        [Serializable]
        private class TimeResponse
        {
            public string datetime;
        }

        private DateTime _serverTime;
        private float _lastSyncedTime;

        public override async UniTask Init()
        {
            GetTimeRequest();
        }

        private async UniTask GetTimeRequest()
        {
            UnityWebRequest request = UnityWebRequest.Get("https://worldtimeapi.org/api");

            await request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.LogError($"Error: {request.error}");
            }
            else
            {
                var response = JsonUtility.FromJson<TimeResponse>(request.downloadHandler.text);
                _serverTime = DateTime.Parse(response.datetime);
                _lastSyncedTime = Time.time;

                Debug.Log($"Server Time: {_serverTime}");
                Debug.Log($"Last Synced Time: {_lastSyncedTime}");
            }
        }
    }
}
