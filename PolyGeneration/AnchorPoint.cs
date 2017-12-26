using UnityEngine;
using System.Collections.Generic;
using UShape.Libs;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace UShape.PolyGeneration
{
    [System.Serializable]
    public struct AnchorPoint
    {
        public Vector2 anchor;
        public Vector2 offset;
        public Vector2 CalcPos(Rect rect)
        {
            return Vector2.Scale(rect.size, anchor) + rect.position + offset;
        }
        public void SetOffset(Rect rect, Vector2 position)
        {
            offset = position - Vector2.Scale(rect.size, anchor) - rect.position;
        }
        public void SetAnchorWithSafePosition(Vector2 newAnchor, Rect rect)
        {
            var op = Vector2.Scale(rect.size, anchor) + offset;
            anchor = newAnchor;
            offset = op - Vector2.Scale(rect.size, anchor);
        }
    }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(AnchorPoint))]
    public class NiceAnchorPointDrawer : PropertyDrawer
    {
        private static List<SerializedProperty> props = new List<SerializedProperty>();
        private static void FillProps(SerializedProperty property)
        {
            props.Clear();
            props.Add(property.FindPropertyRelative("offset"));
            props.Add(property.FindPropertyRelative("anchor"));
        }
        const float bh = 18;
        private RectTransform GetRectTransform(SerializedProperty property)
        {
            if (property.serializedObject.targetObjects.Length > 1)
            {
                return null;
            }
            var comp = property.serializedObject.targetObject as Component;
            if (comp == null) return null;
            return comp.GetComponent<RectTransform>();
        }
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {


            EditorGUI.BeginProperty(position, label, property);
            FillProps(property);
            var h = EditorGUIExtensions.DrawPropertesWithLabelProperty(props, property, position);
            var rectTransf = GetRectTransform(property);
            if (rectTransf != null && property.isExpanded)
            {
                var y = position.y + h;
                var pw = position.width * 0.7f - 8f;
                var x = position.x + position.width - pw - 4f;
                var bw = pw / 6f;

                var rect = rectTransf.rect;

                var anchorProp = property.FindPropertyRelative("anchor");
                var offsetProp = property.FindPropertyRelative("offset");
                var anchorPoint = new AnchorPoint() { anchor = anchorProp.vector2Value, offset = offsetProp.vector2Value };

                //↑←→↓↔↕○□
                if (GUI.Button(new Rect(x, y, bw, bh), "←"))
                {
                    anchorPoint.SetAnchorWithSafePosition(new Vector2(0, anchorPoint.anchor.y), rect);
                }
                x += bw;

                if (GUI.Button(new Rect(x, y, bw, bh), "↑"))
                {
                    anchorPoint.SetAnchorWithSafePosition(new Vector2(anchorPoint.anchor.x, 1), rect);
                }
                x += bw;
                if (GUI.Button(new Rect(x, y, bw, bh), "↓"))
                {
                    anchorPoint.SetAnchorWithSafePosition(new Vector2(anchorPoint.anchor.x, 0), rect);
                }
                x += bw;
                if (GUI.Button(new Rect(x, y, bw, bh), "→"))
                {
                    anchorPoint.SetAnchorWithSafePosition(new Vector2(1, anchorPoint.anchor.y), rect);
                }
                x += bw + 3f;
                if (GUI.Button(new Rect(x, y, bw, bh), "↔"))
                {
                    var nax = (anchorPoint.CalcPos(rect) - rect.position).x / rect.width;
                    anchorPoint.anchor.x = nax;
                    anchorPoint.offset.x = 0;
                }
                x += bw;
                if (GUI.Button(new Rect(x, y, bw, bh), "↕"))
                {
                    var nay = (anchorPoint.CalcPos(rect) - rect.position).y / rect.height;
                    anchorPoint.anchor.y = nay;
                    anchorPoint.offset.y = 0;
                }

                anchorProp.vector2Value = anchorPoint.anchor;
                offsetProp.vector2Value = anchorPoint.offset;
            }

            EditorGUI.EndProperty();
        }
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            FillProps(property);
            var h = EditorGUIExtensions.GetPropertesHeightWithLabelProperty(props, property);
            if (GetRectTransform(property) != null && property.isExpanded)
            {
                h += bh;
            }
            return h;
        }
    }
#endif
}
