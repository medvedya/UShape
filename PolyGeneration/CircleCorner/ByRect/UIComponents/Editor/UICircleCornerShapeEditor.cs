using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UShape.Libs;

namespace UShape.PolyGeneration.CircleCorner.ByRect.UIComponents
{
    [CustomEditor(typeof(UICircleCornerShape), true), CanEditMultipleObjects]
    public class UICircleCornerShapeEditor : Editor
    {
        UICircleCornerShape comp;
        UniversalRectSideCornerListProviderProppertyDrawer sideCornerList;
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            comp = serializedObject.targetObject as UICircleCornerShape;
            if (sideCornerList == null)
                sideCornerList = new UniversalRectSideCornerListProviderProppertyDrawer(this.serializedObject.FindProperty("sideCornerListProvider"));
            sideCornerList.DoLayout();

            serializedObject.ApplyModifiedProperties();

        }
        public static void RectSideCornerListHandler(Transform transform, Rect rect, IRectSideCornerListProvider listProvider, UnityEngine.Object recObj = null)
        {
            if (recObj == null) recObj = listProvider as Object;
            var list = listProvider.SideCornerList;
            for (int i = 0; i < list.Count; i++)
            {
                var side = list[i].side;
                if (side.mode == SideMode.CubicBezierCurve)
                {

                    EditorGUI.BeginChangeCheck();

                    var sideP1 = SoapHandles.AnchorPointHandler(side.p1, rect, transform, i + "-t1");
                    var sideP2 = SoapHandles.AnchorPointHandler(side.p2, rect, transform, i + "-t2");
                    if (EditorGUI.EndChangeCheck())
                    {
                        Undo.RecordObject(recObj, "update side");
                        side.p1 = sideP1;
                        side.p2 = sideP2;
                        list[i].side = side;
                    }

                    {

                        var c1 = list[i].corner.ToCorner(rect);
                        var c2 = list[(i + 1) % list.Count].corner.ToCorner(rect);
                        var p1 = sideP1.CalcPos(rect);
                        var p2 = sideP2.CalcPos(rect);


                        Handles.DrawPolyLine(new Vector3[]
                        {
                                   transform.TransformPoint( MathHelper.CalcCircleTangentDirection(p1,c1.position,c1.radius,true) * c1.radius + c1.position),
                                   transform.TransformPoint( p1),
                                   transform.TransformPoint( p2),
                                   transform.TransformPoint( MathHelper.CalcCircleTangentDirection(p2,c2.position,c2.radius,false) * c2.radius + c2.position),

                        });
                    }
                }
                {

                    EditorGUI.BeginChangeCheck();
                    var corner = list[i].corner;
                    var circleP = SoapHandles.AnchorPointHandler(corner.position, rect, transform, i + "-c");
                    if (EditorGUI.EndChangeCheck())
                    {
                        Undo.RecordObject(recObj, "update corner");
                        corner.position = circleP;
                        list[i].corner = corner;
                    }
                }
            }
        }

        protected void OnSceneGUI()
        {
            if (comp == null) return;
            RectSideCornerListHandler(comp.transform, comp.GetComponent<RectTransform>().rect, comp);
            comp.Generate();
        }
    }
}
