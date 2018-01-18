#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
namespace UShape.Libs
{
    public class EditorGUIExtensions
    {

        public static float DrawProppertyWithChildren(SerializedProperty prop, Rect position, float pading = 1f)
        {
            var h = EditorGUI.GetPropertyHeight(prop);
            EditorGUI.PropertyField(new Rect(position.x, position.y, position.width, h), prop, true);
            return h + pading;
        }
        public static bool DrawProppertyWithOutChildren(SerializedProperty prop, Rect position, ref float rh, float pading = 1f)
        {
            var h = EditorGUI.GetPropertyHeight(prop, false);
            var b = EditorGUI.PropertyField(new Rect(position.x, position.y, position.width, h), prop, false);
            rh += h + pading;
            return b;
        }
        public static float DrawPropertes(IEnumerable<SerializedProperty> propertes, Rect position, float pading = 1f)
        {
            float rh = 0;
            foreach (var item in propertes)
            {
                var h = EditorGUI.GetPropertyHeight(item);
                EditorGUI.PropertyField(new Rect(position.x, position.y, position.width, h), item, true);
                position.y += h + pading;
                rh += h + pading;
            }
            return rh;
        }
        public static float GetPropertesHeight(List<SerializedProperty> propertes, float pading = 1f)
        {
            var y = 0f;
            for (int i = 0; i < propertes.Count; i++)
            {
                var item = propertes[i];
                y += EditorGUI.GetPropertyHeight(item, true) + pading;
            }
            return y;
        }
        public static float DrawPropertesWithLabelProperty(IEnumerable<SerializedProperty> propertes, SerializedProperty labelProperty, Rect position, float pading = 1f)
        {
            var h = EditorGUI.GetPropertyHeight(labelProperty, false);
            if (EditorGUI.PropertyField(new Rect(position.x, position.y, position.width, h), labelProperty, false))
            {
                position.y += h;
                position.x += 10;
                position.width -= 10;
                h += DrawPropertes(propertes, position);
            }
            return h;
        }
        public static float GetPropertesHeightWithLabelProperty(IEnumerable<SerializedProperty> propertes, SerializedProperty labelProperty, float pading = 1f)
        {
            var h = EditorGUI.GetPropertyHeight(labelProperty, false) + pading;
            if (labelProperty.isExpanded)
            {
                foreach (var item in propertes)
                {
                    h += EditorGUI.GetPropertyHeight(item) + pading;
                }
            }
            return h;
        }

    }
}
#endif

