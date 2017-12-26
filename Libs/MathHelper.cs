using UnityEngine;
namespace UShape.Libs
{
    public static class MathHelper
    {
        public static Vector2 CalcCircleTangentDirection(Vector2 point, Vector2 circlePosition, float circleRadius, bool revers = false)
        {

            float a = CalcCircleTangentAngle(point, circlePosition, circleRadius, revers);
            return new Vector2(Mathf.Cos(a), Mathf.Sin(a));
        }

        public static float CalcCircleTangentAngle(Vector2 point, Vector2 circlePosition, float circleRadius, bool revers = false)
        {
            float a = circleRadius;
            float b = Vector2.Distance(point, circlePosition);
            // float c = Mathf.Sqrt(a * a + b * b);
            float A = Mathf.Asin(a / b);
            float AA = (A + Mathf.PI / 2f);
            if (revers) AA *= -1f;
            Vector2 dirFromPointToCircle = (circlePosition - point).normalized;
            float r_a = Mathf.Atan2(dirFromPointToCircle.y, dirFromPointToCircle.x) - AA;
            return Mathf.Repeat(r_a, Mathf.PI * 2f);
        }
        public static Vector2 CalcCircleTangentDirection(Vector2 circlePos1, float circleRadius1, Vector2 circlePos2, float circleRadius2)
        {
            float a = CalcCircleTangentAngle(circlePos1, circleRadius1, circlePos2, circleRadius2);
            return new Vector2(Mathf.Cos(a), Mathf.Sin(a));

        }
        public static Vector2 CalcDirByAngle(float angle)
        {
            return new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        }
        public static float CalcCircleTangentAngle(Vector2 circlePos1, float circleRadius1, Vector2 circlePos2, float circleRadius2)
        {

            float mCircle = (circleRadius2 - circleRadius1);
            float a = CalcCircleTangentAngle(circlePos1, circlePos2, mCircle);
            return a;

        }
    }
}
