using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace UShape.Libs
{
    public class NoPrefixLabelAttribute : PropertyAttribute
    {

    }
#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(NoPrefixLabelAttribute))]
    public class NoPrefixLabelDrawer : PropertyDrawer
    {
        static List<SerializedProperty> myProps = new List<SerializedProperty>();
        private void FillProps(SerializedProperty property)
        {
            myProps.Clear();
            myProps.AddRange(property.GetChildren());
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            FillProps(property);
            EditorGUIExtensions.DrawPropertes(myProps, position);
            EditorGUI.EndProperty();
        }
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            FillProps(property);
            return EditorGUIExtensions.GetPropertesHeight(myProps);
        }
    }
#endif
}