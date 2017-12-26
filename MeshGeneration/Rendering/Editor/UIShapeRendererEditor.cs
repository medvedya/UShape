using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.UI;

namespace UShape.MeshGeneration.Rendering
{
    [CustomEditor(typeof(UIShapeRenderer), true)]
    public class UIShapeRendererEditor : GraphicEditor
    {
        UIShapeRenderer renderer;
        MGNodeSetEditor nodeSetEditor;
        protected override void OnEnable()
        {
            renderer = this.target as UIShapeRenderer;
            nodeSetEditor = new MGNodeSetEditor(serializedObject.FindProperty("nodeSet"), renderer.nodeSet);
            base.OnEnable();
        }
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_Texture"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("polyShapeProvider"));

            AppearanceControlsGUI();
            RaycastControlsGUI();
            EditorGUI.BeginChangeCheck();
            nodeSetEditor.DoLayout();
            if (EditorGUI.EndChangeCheck())
            {

                Undo.RecordObject(target, "change nodeSet");
            }
            serializedObject.ApplyModifiedProperties();
            renderer.SetAllDirty();
        }
        private void OnSceneGUI()
        {
            renderer.SetMeshAsDirty();
        }
    }
}
