using Sources.DataBaseSystem;
using Sources.Utils.Singleton;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Screens.GamePlayScreen
{
    public class ShowJoystickFTUE : MonoBehaviour
    {
        private DataBase _dataBase => Locator<DataBase>.Instance;
        private BuildConfig _buildConfig => _dataBase.GetConfig<BuildConfig>();

        [SerializeField] private GameObject _joystick;
        [SerializeField] private GameObject _btnShoot;

        private void Awake()
        {
            _joystick.SetActive(_buildConfig.UseJoystick);
            _btnShoot.SetActive(_buildConfig.UseJoystick);
        }
    }
}