using UnityEngine;

namespace Extensions
{
    public static class Vector3Extensions
    {
        public static float XZDistance(this Vector3 a, Vector3 b)
        {
            a.y = 0;
            b.y = 0;
            return Vector3.Distance(a, b);
        }
    }
}