using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace UShape.PolyGeneration
{
    public static class SoapHandles
    {


        public static AnchorPoint AnchorPointHandler(AnchorPoint point, Rect rect, Transform transform, string label)
        {
            var anchorWorldPos = transform.TransformPoint(Vector2.Scale(rect.size, point.anchor) + rect.position);
            var worldPos = transform.TransformPoint(point.CalcPos(rect));

            var dir1 = transform.up;
            var dir2 = transform.right;
            {
                Vector3 newPos = Handles.Slider2D(anchorWorldPos, transform.forward, dir1, dir2, HandleUtility.GetHandleSize(anchorWorldPos) * 0.2f, AnchorCap, 100f);
                Vector2 newPosLocal = (Vector2)transform.InverseTransformPoint(newPos) - rect.position;

                point.anchor = new Vector2(newPosLocal.x / rect.size.x, newPosLocal.y / rect.size.y);
                Handles.Label(newPos, "     " + label);

            }
            {
                Vector3 newPos = Handles.Slider2D(worldPos, transform.forward, dir1, dir2, HandleUtility.GetHandleSize(worldPos) * 0.1f, PosCap, 100f);
                point.SetOffset(rect, transform.InverseTransformPoint(newPos));
            }

            var tmpColor = Handles.color;
            Handles.color = Color.grey;
            Handles.DrawLine(anchorWorldPos, worldPos);
            Handles.color = tmpColor;

            return point;
        }
        static Vector3[] rectCashe = new Vector3[5];
        private static void AnchorCap(int controlId, Vector3 position, Quaternion rotation, float size, EventType eventType)
        {
            {
                Vector3 b = rotation * new Vector3(size * 1.1f, 0f, 0f);
                Vector3 b2 = rotation * new Vector3(0f, size * 1.1f, 0f);
                rectCashe[0] = position + b + b2;
                rectCashe[1] = position + b - b2;
                rectCashe[2] = position - b - b2;
                rectCashe[3] = position - b + b2;
                rectCashe[4] = position + b + b2;
                var tmp = Handles.color;
                Handles.color = Color.black;
                Handles.DrawPolyLine(rectCashe);
                Handles.color = tmp;
            }
            Handles.RectangleHandleCap(controlId, position, rotation, size, eventType);
        }
        private static void PosCap(int controlId, Vector3 position, Quaternion rotation, float size, EventType eventType)
        {

            Handles.CircleHandleCap(controlId, position, rotation, size, eventType);
            var tmp = Handles.color;
            Handles.color = Color.black;
            Handles.DrawWireDisc(position, rotation * Vector3.forward, size * 1.1f);
            Handles.color = tmp;
        }
    }
}
