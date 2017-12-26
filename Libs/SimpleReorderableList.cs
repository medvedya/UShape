#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditorInternal;
using UnityEditor;
namespace UShape.Libs
{
    public class SimpleReorderableList
    {

        ReorderableList rList;
        SerializedProperty elements;
        public SimpleReorderableList(SerializedObject serializedObject, SerializedProperty elements, bool showElementHeaders = true)
        {
            this.elements = elements;
            rList = new ReorderableList(serializedObject, elements, true, true, true, true)
            {
                drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
                  {
                      rect.x += 20;
                      rect.width -= 20;
                      var element = elements.GetArrayElementAtIndex(index);
                      if (showElementHeaders)
                      {
                          var h = EditorGUI.GetPropertyHeight(element);
                          EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width, h), element, true);
                      }
                  },
                elementHeightCallback = (int index) =>
                  {
                      float h = 0;
                      if (showElementHeaders)
                      {
                          h = EditorGUI.GetPropertyHeight(elements.GetArrayElementAtIndex(index));
                      }
                      return h;
                  },
                drawHeaderCallback = (Rect rect) =>
                {
                    float h = EditorGUI.GetPropertyHeight(elements, false);
                    rect.x += 10;
                    rect.width -= 10;
                    EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width, h), elements, false);
                }
            };
        }
        public void DoLayoutList()
        {
            if (elements.isExpanded)
            {
                rList.DoLayoutList();
            }
            else
            {
                EditorGUILayout.PropertyField(elements, false);
            }
        }
    }
#endif
}