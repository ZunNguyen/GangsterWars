using Cysharp.Threading.Tasks;
using Sources.DataBaseSystem;
using Sources.SystemService;
using Sources.Utils.Singleton;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sources.GamePlaySystem.Joystick
{
    public class JoystickSystem : BaseSystem
    {
        private DataBase _dataBase => Locator<DataBase>.Instance;
        private BuildConfig _buildConfig => _dataBase.GetConfig<BuildConfig>();

        public bool IsUseJoystick { get; private set; }

        public override async UniTask Init()
        {
            SetUseJoystick();
        }

        private void SetUseJoystick()
        {
            IsUseJoystick = _buildConfig.IsJoystick;
        }
    }
}