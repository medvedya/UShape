using UnityEngine;
namespace UShape.Libs
{
    public struct CubicBezier
    {
        public Vector2 p0;
        public Vector2 p1;
        public Vector2 p2;
        public Vector2 p3;

        public Vector2 CalculateCubicBezierPoint(float t)
        {
            float u = 1 - t;
            float tt = t * t;
            float uu = u * u;
            float uuu = uu * u;
            float ttt = tt * t;

            Vector2 p = uuu * p0;
            p += 3 * uu * t * p1;
            p += 3 * u * tt * p2;
            p += ttt * p3;

            return p;
        }
        public Vector2 CalculateCubicBezierDerivative(float t)
        {
            /* -3 * P0 * (1 - t) ^ 2 +
     P1 * (3 * (1 - t) ^ 2 - 6 * (1 - t) * t) +
     P2 * (6 * (1 - t) * t - 3 * t ^ 2) +
     3 * P3 * t ^ 2*/
            var k = (1f - t);
            var kk = k * k;
            var tt = t * t;
            return -3f * p0 * kk + p1 * (3f * kk - 6f * k * t) + p2 * (6f * k * t - 3f * tt) + 3f * p3 * tt;
        }
    }
}
