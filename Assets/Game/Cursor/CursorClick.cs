using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Cursor
{
    public class CursorClick : MonoBehaviour
    {
        [SerializeField] private CursorIconHandler _cursorIconHandler;

        public void OnClick()
        {
            _cursorIconHandler.CursorClick();
        }
    }
}