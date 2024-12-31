using Sources.GamePlaySystem.Joystick;
using Sources.Utils.Singleton;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Cursor
{
    public class CursorClick : MonoBehaviour
    {
        private JoystickSystem _joystickSystem => Locator<JoystickSystem>.Instance;

        [SerializeField] private CursorIconHandler _cursorIconHandler;

        private void Awake()
        {
            if (!_joystickSystem.IsUseJoystick) gameObject.SetActive(false);
        }

        public void OnClick()
        {
            _cursorIconHandler.CursorClick();
        }
    }
}