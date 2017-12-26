using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UShape.Libs;

namespace UShape.MeshGeneration
{
    [CustomPropertyDrawer(typeof(ComplexMGNodes.UniversalEdge))]
    public class UniversalEdgeDrawer : PropertyDrawer
    {
        static List<SerializedProperty> myProps = new List<SerializedProperty>();
        void FillProps(SerializedProperty property)
        {
            myProps.Clear();
            var modeProp = property.FindPropertyRelative("mode");
            myProps.Add(modeProp);
            var usePaintProp = property.FindPropertyRelative("usePaint");
            myProps.Add(property.FindPropertyRelative("relative"));
            myProps.Add(usePaintProp);
            myProps.Add(property.FindPropertyRelative("extrude1"));
            var usePaint = usePaintProp.boolValue;
            if (usePaint)
            {
                myProps.Add(property.FindPropertyRelative("paint1"));
            }
            if (modeProp.enumValueIndex == (int)ComplexMGNodes.UniversalEdge.ComplexAddEdgeMode.Line)
            {
                myProps.Add(property.FindPropertyRelative("extrude2"));
                if (usePaint)
                {
                    myProps.Add(property.FindPropertyRelative("paint2"));
                }
            }
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
}