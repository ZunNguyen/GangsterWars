using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.Cursor
{
    public class CursorIcon : MonoBehaviour
    {
        private Vector3 moveDirection;

        [SerializeField] private Joystick _joystick;

        private void FixedUpdate()
        {
            float horizontal = _joystick.Horizontal;
            float vertical = _joystick.Vertical;

            // Tạo vector di chuyển
            moveDirection = new Vector3(horizontal, vertical, 0f);

            // Cập nhật vị trí của cursor
            transform.position += moveDirection * 5f * Time.deltaTime;
        }
    }
}