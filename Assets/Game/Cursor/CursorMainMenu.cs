using UnityEngine;

namespace Game.Cursorsd
{
    public class CursorMainMenu : MonoBehaviour
    {
        [SerializeField] private Texture2D _cursor;

        private void Start()
        {
            UnityEngine.Cursor.SetCursor(_cursor, Vector2.zero, CursorMode.Auto);
        }
    }
}