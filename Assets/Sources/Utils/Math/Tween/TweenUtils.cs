using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sources.Utils
{
    public static class TweenUtils
    {
        public static float GetTimeDuration(Vector3 pos_1, Vector3 pos_2, float speed)
        {
            var S = Vector3.Distance(pos_1, pos_2);
            return S / speed;
        }
    }
}