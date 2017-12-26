using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UShape.Libs;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace UShape.MeshGeneration
{
    public abstract class MGEdgeModifierNode : IMGNode
    {
        public int edgeCount = 1;
        public void Do(MeshGenerationContext context)
        {
            int count = this.edgeCount;
            if (count < 0) count = context.edges.Count;
            if (count > context.edges.Count) count = context.edges.Count;
            int startEdgeIndex = context.edges.Count - count;
            if (startEdgeIndex < 0) startEdgeIndex = 0;

            var startMeshIndex = context.mesh.vertexes.Count;

            for (int i = 0; i < count; i++)
            {
                startMeshIndex -= context.edges[startEdgeIndex + i].polyShape.Count;
            }

            for (int i = 0; i < count; i++)
            {
                var edge = context.edges[startEdgeIndex + i];
                Do(context.mesh, startMeshIndex, context.polyShape.Count, context.polyShape);
                startMeshIndex += context.polyShape.Count;
            }
        }
        protected abstract void Do(DynamicMesh mesh, int startMeshIndex, int vertexCount, PolyShape poly);
    }
    public class MGEdgeModifierNodeWithoutEdgeCountAttribute : PropertyAttribute
    {

    }
#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(MGEdgeModifierNodeWithoutEdgeCountAttribute), true)]
    class MGEdgeModifierNodeWithoutEdgeCountDrawer : PropertyDrawer
    {
        static List<SerializedProperty> props = new List<SerializedProperty>();
        static void FillProps(SerializedProperty property)
        {
            props.Clear();
            foreach (var item in property.GetChildren())
            {
                if (item.name != "edgeCount")
                {
                    props.Add(item);
                }
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
