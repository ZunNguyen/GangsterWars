using UnityEngine;

namespace Sources.Utils
{
    public static class GetVector
    {
        public static Vector3 GetHightPointBetweenTwoPoint(Vector3 point_1, Vector3 point_2, float height)
        {
            Vector3 midPoint = (point_1 + point_2) / 2;

            Vector3 direction = (point_1 - point_2).normalized;

            Vector3 normal = Vector3.Cross(direction, Vector3.up).normalized;

            midPoint += normal * height;
            midPoint.z = point_2.z;

            return midPoint;
        }
    }
}

