using Cysharp.Threading.Tasks;
using Sources.DataBaseSystem;
using Sources.GameData;
using Sources.SystemService;
using Sources.Utils.Singleton;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace Sources.GamePlaySystem.Joystick
{
    public class InitJoystickSystemService : InitSystemService<JoystickSystem> { }

    public class JoystickSystem : BaseSystem
    {
        private GameData.GameData _gameData => Locator<GameData.GameData>.Instance;
        private UserSettingProfile _userSettingProfile => _gameData.GetProfileData<UserSettingProfile>();

        private DataBase _dataBase => Locator<DataBase>.Instance;
        private BuildConfig _buildConfig => _dataBase.GetConfig<BuildConfig>();

        public bool IsUseJoystick { get; private set; }
        public ReactiveProperty<float> CursorSensitivity { get; private set; } = new ();

        public override async UniTask Init()
        {
            SetUseJoystick();
#if UNITY_ANDROID || UNITY_IOS
            GetCursorSensitivity();
#endif
        }

        private void SetUseJoystick()
        {
            IsUseJoystick = _buildConfig.UseJoystick;
        }

#if UNITY_ANDROID || UNITY_IOS
        private void GetCursorSensitivity()
        {
            CursorSensitivity.Value = _userSettingProfile.CursorSensitivity;
        }

        public void SetCursorSensitivity(float value)
        {
            CursorSensitivity.Value = value;
            _userSettingProfile.SetCursorSensitivity(value);
        }
#endif
    }
}