using System.Collections.Generic;
using UnityEngine;
using UShape.Libs;
namespace UShape.PolyGeneration.CircleCorner
{
    public static class CircleCornerShapeDrawer
    {
        public static void DrawCircleCornerPolygon(this PolyShape polyShape, List<Corner> cornerList, List<Side> sideList)
        {
            var count = cornerList.Count;

            for (int i = 0; i < cornerList.Count; i++)
            {
                var curSide = sideList[i];
                var predSide = sideList[(i - 1 + count) % count];
                var predCorner = cornerList[(i - 1 + count) % count];
                var curCorner = cornerList[i];
                var nextCorner = cornerList[(i + 1) % count];
                float startAngle = 0;
                float length = 0;
                {
                    float endAngle = 0;
                    if (predSide.mode == SideMode.Flat)
                    {
                        startAngle = MathHelper.CalcCircleTangentAngle(predCorner.position, predCorner.radius, curCorner.position, curCorner.radius);
                    }
                    else
                    {
                        startAngle = MathHelper.CalcCircleTangentAngle(predSide.p2, curCorner.position, curCorner.radius);
                    }


                    if (curSide.mode == SideMode.Flat)
                    {
                        endAngle = MathHelper.CalcCircleTangentAngle(curCorner.position, curCorner.radius, nextCorner.position, nextCorner.radius);
                    }
                    else
                    {
                        endAngle = MathHelper.CalcCircleTangentAngle(curSide.p1, curCorner.position, curCorner.radius, true);

                    }
                    endAngle = Mathf.Repeat(endAngle, Mathf.PI * 2f);

                    length = endAngle - startAngle;
                    if (length < 0f) length = Mathf.PI * 2f + length;
                    polyShape.DrawArc(curCorner.position, curCorner.radius, startAngle, length, curCorner.segmentCount);

                    if (curSide.mode == SideMode.CubicBezierCurve)
                    {
                        polyShape.DrawCubicBezier(new CubicBezier()
                        {
                            p0 = MathHelper.CalcDirByAngle(endAngle) * curCorner.radius + curCorner.position,
                            p1 = curSide.p1,
                            p2 = curSide.p2,
                            p3 = MathHelper.CalcDirByAngle(MathHelper.CalcCircleTangentAngle(curSide.p2, nextCorner.position, nextCorner.radius)) * nextCorner.radius + nextCorner.position,
                        }, curSide.segmentCount);
                    }

                }
            }
            if (cornerList.Count > 1)
            {
                polyShape.Add(polyShape[0]);
            }
        }
    }
}

