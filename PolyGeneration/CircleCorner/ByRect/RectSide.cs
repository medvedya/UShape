using UnityEngine;
using System.Collections.Generic;
using UShape.Libs;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace UShape.PolyGeneration.CircleCorner.ByRect
{
    [System.Serializable]
    public struct RectSide
    {
        public AnchorPoint p1;
        public AnchorPoint p2;
        public SideMode mode;
        public int segmentCount;
        public Side ToSide(Rect rect)
        {
            return new Side()
            {
                p1 = this.p1.CalcPos(rect),
                p2 = this.p2.CalcPos(rect),
                mode = mode,
                segmentCount = segmentCount
            };
        }

    }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(RectSide))]
    public class RectSideDrawer : PropertyDrawer
    {
        static List<SerializedProperty> props = new List<SerializedProperty>();
        private static void FillProps(SerializedProperty property)
        {
            props.Clear();
            var sideProp = property.FindPropertyRelative("mode");
            props.Add(sideProp);
            var side = (SideMode)sideProp.enumValueIndex;
            if (side == SideMode.CubicBezierCurve)
            {
                props.Add(property.FindPropertyRelative("segmentCount"));
                props.Add(property.FindPropertyRelative("p1"));
                props.Add(property.FindPropertyRelative("p2"));
            }
        }
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            FillProps(property);
            EditorGUIExtensions.DrawPropertesWithLabelProperty(props, property, position);
            EditorGUI.EndProperty();
        }
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            FillProps(property);
            return EditorGUIExtensions.GetPropertesHeightWithLabelProperty(props, property);

        }

    }
#endif
}

