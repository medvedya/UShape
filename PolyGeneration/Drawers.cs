using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UShape.Libs;

namespace UShape.PolyGeneration
{
    public static class Drawers
    {
        public static void DrawCubicBezier(this PolyShape polyShape, CubicBezier bezier, int segmentCount)
        {
            float segmentSize = 1f / segmentCount;
            for (int i = 0; i < segmentCount; i++)
            {
                var t = i * segmentSize;
                var dir = bezier.CalculateCubicBezierDerivative(t).normalized;
                Point p = new Point()
                {
                    position = bezier.CalculateCubicBezierPoint(t),
                    normal = new Vector2(dir.y, -dir.x)
                };
                polyShape.TryAddPoint(p);
            }
        }

        const float PI2 = Mathf.PI * 2f;

        public static void DrawArc(this PolyShape polyShape, Vector2 position, float raduis, float startAngle, float length, int segmentCountForFull)
        {
            int segmentCount = (int)(length / (PI2 / segmentCountForFull));
            float segmentSize = length / (float)segmentCount;
            for (int i = 0; i <= segmentCount; i++)
            {
                float a = i * segmentSize + startAngle;
                var normal = new Vector2(Mathf.Cos(a), Mathf.Sin(a));
                var point = new Point()
                {
                    normal = normal,
                    position = position + raduis * normal
                };
                polyShape.TryAddPoint(point);
            }
        }
        public static void DrawEllipse(this PolyShape polyShape, Rect rect, int segmentCount)
        {
            float segmentSize = PI2 / (float)segmentCount;
            for (int i = 0; i < segmentCount; i++)
            {
                float a = i * segmentSize;
                var dir = new Vector2(Mathf.Cos(a) / 2f, Mathf.Sin(a) / 2);
                var point = new Point()
                {
                    position = Vector2.Scale(dir, rect.size) + rect.position + rect.size / 2f,
                    normal = dir
                };
                polyShape.Add(point);
            }
        }
    }
}
