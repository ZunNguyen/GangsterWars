using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using Sources.GameData;
using Sources.SystemService;
using Sources.Utils.Singleton;
using System;
using UnityEngine;
using UnityEngine.Networking;
using UnityEditor;
using UniRx;

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

        private DateTime _timeLogin;

        public int DurationTimeOffline { get; private set; }
        public bool IsCompleteSetTimeLogin { get; private set; } = false;
        public Action AddOneSecondTimeOnline;

        public override async UniTask Init()
        {
            GetTimeLogin();
            SetPackEarnCoinProfile();
            SetDurationTimeOffline();
            SetDurationTimeOnline();

#if UNITY_EDITOR
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
#endif
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

            IsCompleteSetTimeLogin = true;
        }

        private async void SetPackEarnCoinProfile()
        {
            await UniTask.WaitUntil(() => IsCompleteSetTimeLogin);

            if (_packEarnCoinProfile.PackEarnCoinDatas == null)
            {
                _packEarnCoinProfile.SetDataDefault();
                _packEarnCoinProfile.SetLastTimeUserPlay(_timeLogin);
            }
        }

        private async void SetDurationTimeOffline()
        {
            await UniTask.WaitUntil(() => IsCompleteSetTimeLogin);

            var durationTime = _timeLogin - _packEarnCoinProfile.LastTimeUserPlay;
            DurationTimeOffline = (int)durationTime.TotalSeconds;

            Debug.Log($"DurationTimeOffline: {DurationTimeOffline}");
        }

        private async void SetDurationTimeOnline()
        {
            while (true)
            {
                await UniTask.Delay(TimeSpan.FromSeconds(1));
                AddOneSecondTimeOnline?.Invoke();
            }
        }

        private void OnApplicationQuit()
        {
            _packEarnCoinProfile.SetLastTimeUserPlay(_timeLogin);
        }

#if UNITY_EDITOR
        private void OnPlayModeStateChanged(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.ExitingPlayMode)
            {
                _packEarnCoinProfile.SetLastTimeUserPlay(_timeLogin);
            }
        }
#endif
    }
}