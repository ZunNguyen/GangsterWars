using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.CanvasInGamePlay.HPBar
{
    public class MoveMent : MonoBehaviour
    {
        public RectTransform rectTransformObject; // Đối tượng có RectTransform
        public Transform worldTransformObject; // Đối tượng có Transform
        public Canvas canvas; // Canvas mà RectTransform thuộc về

        void Update()
        {
            // Chuyển đổi vị trí từ World Space sang Screen Space
            Vector3 screenPos = RectTransformUtility.WorldToScreenPoint(Camera.main, worldTransformObject.position);

            // Chuyển đổi Screen Space sang vị trí trong RectTransform
            Vector2 anchoredPos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, screenPos, canvas.worldCamera, out anchoredPos);

            // Đặt vị trí cho RectTransform
            rectTransformObject.anchoredPosition = anchoredPos;
        }
    }
}