using UnityEngine;

namespace Sources.Utils
{
    public static class GetVector
    {
        public static Vector3 GetHightPointBetweenTwoPoint(Vector3 point_1, Vector3 point_2, float height)
        {
            Vector2 point1 = new Vector2(point_1.x, point_1.y);
            Vector2 point2 = new Vector2(point_2.x, point_2.y);

            Vector2 midPoint = (point1 + point2) / 2;

            Vector2 direction = (point1 - point2).normalized;
            Vector2 normal = new Vector2(-direction.y, direction.x);

            midPoint += normal * height;

            return new Vector3(midPoint.x, midPoint.y, point_1.z);
        }
    }
}

