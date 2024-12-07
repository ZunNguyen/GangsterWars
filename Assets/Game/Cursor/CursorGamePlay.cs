using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Cursor
{
    public class CursorGamePlay : MonoBehaviour
    {
        [SerializeField] private Texture2D _cursor;

        private Vector2 _cursorHotspot;

        private void Start()
        {
            _cursorHotspot = new Vector2(_cursor.width / 2, _cursor.height / 2);
            UnityEngine.Cursor.SetCursor(_cursor, _cursorHotspot, CursorMode.Auto);
        }
    }
}