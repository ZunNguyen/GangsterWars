using Sources.DataBaseSystem;
using Sources.GamePlaySystem.Joystick;
using Sources.Utils.Singleton;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Screens.GamePlayScreen
{
    public class SettingCursorHandler : MonoBehaviour
    {
        private DataBase _dataBase => Locator<DataBase>.Instance;
        private BuildConfig _buildConfig => _dataBase.GetConfig<BuildConfig>();

        private JoystickSystem _joystickSystem => Locator<JoystickSystem>.Instance;

        [SerializeField] private Slider _slider;

        private void Awake()
        {
            gameObject.SetActive(_buildConfig.UseJoystick);

            if (!_buildConfig.UseJoystick) return;
            _slider.value = _joystickSystem.CursorSensitivity.Value;
        }

        public void AdjustCursorSensitivity()
        {
#if UNITY_ANDROID || UNITY_IOS
            _joystickSystem.SetCursorSensitivity(_slider.value);
#endif
        }
    }
}