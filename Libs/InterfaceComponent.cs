using UnityEngine;
using System.Collections.Generic;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace UShape.Libs
{
    public abstract class InterfaceObject
    {
        [SerializeField]
        protected UnityEngine.Object externalObject;
    }
    public class InterfaceObject<T> : InterfaceObject where T : class
    {
        public static System.Type TargetType()
        {
            return typeof(T);
        }
        public T UsedObject
        {
            get
            {
                if (externalObject != null && externalObject is T)
                {
                    return externalObject as T;
                }
                return null;
            }
            set
            {
                var old = UsedObject;
                if (value is UnityEngine.Object)
                {
                    externalObject = value as UnityEngine.Object;
                }
            }
        }
    }
#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(InterfaceObject), true)]
    public class InterfaceObjectDrawer : PropertyDrawer
    {
        public bool FindInThisGameObject = true;
        public bool extensionLabel = true;
        private System.Type TargetType
        {
            get
            {
                return this.fieldInfo.FieldType.BaseType.GetMethod("TargetType").Invoke(null, null) as System.Type;
            }
        }

        private void FillCandidateList(SerializedProperty property)
        {
            candidateList.Clear();
            var prop = property.FindPropertyRelative("externalObject");
            if (prop.objectReferenceValue != null && TargetType.IsAssignableFrom(prop.objectReferenceValue.GetType()))
            {
                return;
            }
            if (prop.objectReferenceValue == null && property.serializedObject.targetObject is Component && property.serializedObject.targetObjects.Length == 1)
            {
                var to = property.serializedObject.targetObject as Component;
                var comps = to.GetComponents(TargetType);
                foreach (var item in comps)
                {
                    if (to != item && item != prop.objectReferenceValue)
                        candidateList.Add(item);
                }
            }
            else if (prop.objectReferenceValue != null)
            {
                if (prop.objectReferenceValue.GetType() == typeof(GameObject))
                {
                    var go = prop.objectReferenceValue as GameObject;
                    candidateList.AddRange(go.GetComponents(TargetType));
                }
            }
        }
        static List<Object> candidateList = new List<Object>();
        const float fieldHeight = 18;
        const float buttonHeight = 20;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {

            EditorGUI.BeginProperty(position, label, property);
            var prop = property.FindPropertyRelative("externalObject");
            float y = position.y;
            label.text += " (" + TargetType.Name + ")";
            EditorGUI.PropertyField(new Rect(position.x, y, position.width, fieldHeight - 2f), prop, label);
            y += fieldHeight;
            FillCandidateList(property);

            if (candidateList.Count == 1 && prop.objectReferenceValue != null)
            {
                prop.objectReferenceValue = candidateList[0];
            }
            if (prop.objectReferenceValue != null && !TargetType.IsAssignableFrom(prop.objectReferenceValue.GetType()) && !(prop.objectReferenceValue is GameObject))
            {
                prop.objectReferenceValue = null;
            }
            foreach (var item in candidateList)
            {
                if (GUI.Button(new Rect(position.x, y, position.width, buttonHeight - 2f), item.ToString()))
                {
                    prop.objectReferenceValue = item;
                    break;
                }
                y += buttonHeight;
            }
            EditorGUI.EndProperty();
        }
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            FillCandidateList(property);
            return fieldHeight + buttonHeight * candidateList.Count;
        }
    }
#endif
}

