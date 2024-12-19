using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using Sources.GameData;
using Sources.SystemService;
using Sources.Utils.Singleton;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

namespace Sources.TimeManager
{
    public class InitTimeManagerSystemService : InitSystemService<TimeManagerSystem> { }

    public class TimeResponse
    {
        public string datetime;
    }

    public class TimeManagerSystem : BaseSystem
    {
        private const string _urlTimeRequest = "https://timeapi.io/api/Time/current/zone?timeZone=UTC";

        private GameData.GameData _gameData => Locator<GameData.GameData>.Instance;
        private PackEarnCoinProfile _packEarnCoinProfile => _gameData.GetProfileData<PackEarnCoinProfile>();

        private bool _isCompleteSetTimeLogin = false;
        private DateTime _timeLogin;

        public int DurationTimeOffline { get; private set; }

        public override async UniTask Init()
        {
            GetTimeLogin();
            SetPackEarnCoinProfile();
            SetDurationTimeOffline();
        }

        private async void GetTimeLogin()
        {
            UnityWebRequest request = UnityWebRequest.Get(_urlTimeRequest);

            await request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Error: {request.error}");
                return;
            }
            else
            {
                var response = JsonConvert.DeserializeObject<TimeResponse>(request.downloadHandler.text);
                _timeLogin = DateTime.Parse(response.datetime);
                Debug.Log($"Server Time: {_timeLogin}");
            }

            _isCompleteSetTimeLogin = true;
        }

        private async void SetPackEarnCoinProfile()
        {
            await UniTask.WaitUntil(() => _isCompleteSetTimeLogin);

            if (_packEarnCoinProfile.PackEarnCoinDatas == null)
            {
                _packEarnCoinProfile.SetDataDefault();
                _packEarnCoinProfile.SetLastTimeUserPlay(_timeLogin);
            }
        }

        private async void SetDurationTimeOffline()
        {
            await UniTask.WaitUntil(() => _isCompleteSetTimeLogin);

            var durationTime = _timeLogin - _packEarnCoinProfile.LastTimeUserPlay;
            DurationTimeOffline = (int)durationTime.TotalMinutes;

            Debug.Log($"DurationTimeOffline: {DurationTimeOffline}");
        }
    }
}