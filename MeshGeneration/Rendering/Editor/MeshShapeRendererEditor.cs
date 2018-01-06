using UnityEditor;

namespace UShape.MeshGeneration.Rendering
{
    [CustomEditor(typeof(MeshShapeRenderer), true)]
    public class MeshShapeRendererEditor:Editor
    {
        MeshShapeRenderer renderer;
        MGNodeSetEditor nodeSetEditor;
        protected void OnEnable()
        {
            renderer = this.target as MeshShapeRenderer;
            nodeSetEditor = new MGNodeSetEditor(serializedObject.FindProperty("nodeSet"), renderer.nodeSet);
        }
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("color"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("polyShapeProvider"));
            EditorGUI.BeginChangeCheck();
            nodeSetEditor.DoLayout();
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(target, "change nodeSet");
            }
            serializedObject.ApplyModifiedProperties();
        }
        private void OnSceneGUI()
        {
            renderer.Draw();
        }
    }
}
