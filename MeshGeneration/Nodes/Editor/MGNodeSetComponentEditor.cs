using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEditor;
namespace UShape.MeshGeneration.Nodes
{
    [CustomEditor(typeof(MGNodeSetComponent), true)]
    public class MGNodeSetComponentEditor:Editor
    {
        MGNodeSetEditor nodeSetEditor;
        protected void OnEnable()
        {
            nodeSetEditor = new MGNodeSetEditor(serializedObject.FindProperty("nodeSet"), (serializedObject.targetObject as MGNodeSetComponent).nodeSet);
        }
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            nodeSetEditor.DoLayout();
            serializedObject.ApplyModifiedProperties();
        }

    }
}
