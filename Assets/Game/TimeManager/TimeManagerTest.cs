using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TimeManagerTest : MonoBehaviour
{
    private DateTime _serverTime;
    private float _lastSyncedTime;

    [Serializable]
    private class TimeResponse
    {
        public string datetime;
    }

    private void Start()
    {
        //GetTimeRequest();
    }

    private async void GetTimeRequest()
    {
        UnityWebRequest request = UnityWebRequest.Get("https://timeapi.io/api/Time/current/zone?timeZone=UTC");

        await request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError($"Error: {request.error}");
            return;
        }
        else
        {
            var response = JsonConvert.DeserializeObject<TimeResponse>(request.downloadHandler.text);

            _serverTime = DateTime.Parse(response.datetime);
            _lastSyncedTime = Time.time;

            Debug.Log($"Server Time: {_serverTime}");
            Debug.Log($"Last Synced Time: {_lastSyncedTime}");
        }
    }
}
