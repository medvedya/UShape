#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using UShape.Libs;

namespace UShape.MeshGeneration
{
    public class MGNodeSetEditor
    {
        ReorderableList rList;
        MGNodeSet nodeSet;
        SerializedProperty nodeSetProperty;
        public MGNodeSetEditor(SerializedProperty nodeSetProperty, MGNodeSet nodeSet)
        {
            this.nodeSet = nodeSet;
            this.nodeSetProperty = nodeSetProperty;
            rList = new ReorderableList(nodeSetProperty.serializedObject, nodeSetProperty.FindPropertyRelative("map"), true, false, true, false)
            {
                drawElementCallback = DrawElementHandler,
                elementHeightCallback = (int index) =>
                {
                    if (index >= nodeSet.map.Count) return 0;

                    var prop = GetNodeProp(index);
                    float h = 20;
                //foreach (var item in prop.GetChildren())
                //{
                //    h += EditorGUI.GetPropertyHeight(item, true);
                //}
                if (prop != null)
                        h += EditorGUI.GetPropertyHeight(prop, true);

                    if (h < 60) h = 60;
                    return h;
                },
                onAddDropdownCallback = (Rect rect, ReorderableList list) =>
                {
                    ShowAddNodeContextMenu();
                },
                drawHeaderCallback = (rect) =>
                {
                    rect.x += 10;
                    rect.width -= 10;
                    EditorGUI.PropertyField(new Rect(rect.x,rect.y,rect.width / 2f,rect.height), nodeSetProperty, false);

                    float bw = rect.width / 2f / 3f;
                    float bx = rect.x + rect.width / 2f;
                    const string mgNodeSetBufferPrefix = "MGNodeSetBuffer";
                    if (!string.IsNullOrEmpty(EditorGUIUtility.systemCopyBuffer) && EditorGUIUtility.systemCopyBuffer.IndexOf(mgNodeSetBufferPrefix) == 0 && GUI.Button(new Rect(bx, rect.y, bw, rect.height), "Paste"))
                    {

                        Undo.RecordObject(nodeSetProperty.serializedObject.targetObject, "Paste");
                        var tmpBuffer = new MGNodeSet();
                        EditorJsonUtility.FromJsonOverwrite(EditorGUIUtility.systemCopyBuffer.Substring(mgNodeSetBufferPrefix.Length),tmpBuffer);
                        tmpBuffer.CopyTo(nodeSet);
                    }
                    bx += bw;
                    if (GUI.Button(new Rect(bx, rect.y, bw, rect.height), "Copy"))
                    {
                        EditorGUIUtility.systemCopyBuffer = mgNodeSetBufferPrefix + EditorJsonUtility.ToJson(nodeSet);
                    }
                    bx += bw;
                    if (GUI.Button(new Rect(bx,rect.y,bw,rect.height),"Clear"))
                    {
                        Undo.RecordObject(nodeSetProperty.serializedObject.targetObject, "Clear");
                        nodeSet.Clear();
                    }


                }
            };

        }
        SerializedProperty GetNodeProp(int index)
        {
            var mapItem = nodeSet.map[index];
            return GetNodePropertyByMapItem(mapItem);
        }
        void DrawElementHandler(Rect rect, int index, bool isActive, bool isFocused)
        {
            if (index >= nodeSet.map.Count) return;
            float y = rect.y;
            var mapItem = nodeSet.map[index];
            var nodeCollection = nodeSet.GetNodeCollection(mapItem.typeCode);
            if (nodeCollection.Count > mapItem.refIndex)
            {
                var node = nodeCollection[mapItem.refIndex];
                var prop = GetNodeProp(index);
                EditorGUI.LabelField(new Rect(rect.x + 5, y, rect.width - 10, EditorGUIUtility.singleLineHeight), GetNameForMenu(nodeCollection));
                EditorGUI.PropertyField(new Rect(rect.width + 20, y, 25, EditorGUIUtility.singleLineHeight), nodeSetProperty.FindPropertyRelative("map").GetArrayElementAtIndex(index).FindPropertyRelative("enabled"),GUIContent.none);
               // mapItem.enabled = EditorGUI.Toggle(new Rect(rect.width + 20, y, 25, EditorGUIUtility.singleLineHeight), mapItem.enabled);
                nodeSet.map[index] = mapItem;
                y += 20;
                float h = EditorGUI.GetPropertyHeight(prop);
                EditorGUI.PropertyField(new Rect(rect.x + 25, y, rect.width - 25, h), prop, true);
            }
            else
            {
                EditorGUI.LabelField(new Rect(rect.x + 5, y, rect.width - 10, EditorGUIUtility.singleLineHeight), "Missed");
            }

            /*
            foreach (var item in prop.GetChildren())
            {
                float h = EditorGUI.GetPropertyHeight(item);
                EditorGUI.PropertyField(new Rect(rect.x + 25, y, rect.width - 25, h), item, true);
                y += h;
            }*/

            if (GUI.Button(new Rect(rect.x - 17, rect.y + 20, 20, 15), "-"))
            {
                Undo.RecordObjects(nodeSetProperty.serializedObject.targetObjects, "Remove MG");
                nodeSet.Remove(index);
            }
            if (GUI.Button(new Rect(rect.x - 17, rect.y + 40, 20, 15), "+"))
            {
                ShowAddNodeContextMenu(index + 1);
            }
        }

        List<MGNodeSet.NodeCollection> res = new List<MGNodeSet.NodeCollection>();
        void ShowAddNodeContextMenu(int index = -1)
        {
            var menu = new GenericMenu();
            res.Clear();
            nodeSet.GetAllNodeCollections(res);
            foreach (var item in res)
            {
                menu.AddItem(new GUIContent(GetNameForMenu(item)), false, AddContextMenuHandler, new ContextMenuClickInfo() { typeCode = item.TypeCode, index = index });
            }
            menu.ShowAsContext();
        }

        string GetNameForMenu(MGNodeSet.NodeCollection collection)
        {
            return ((MGNodeSet.NodeTye)collection.TypeCode).ToString().Replace('_', '/');
        }
        class ContextMenuClickInfo
        {
            public int typeCode;
            public int index;
        }
        void AddContextMenuHandler(object info)
        {
            var ci = info as ContextMenuClickInfo;
            Undo.RecordObjects(nodeSetProperty.serializedObject.targetObjects, "Add MG");
            if (ci.index == -1)
            {
                nodeSet.Add(ci.typeCode);
            }
            else
            {
                nodeSet.Insert(ci.index, ci.typeCode);
            }
        }
        SerializedProperty GetNodePropertyByMapItem(MGNodeSet.MapPare mapItem)
        {

            var na = GetNodeArrayProperty(mapItem.typeCode);
            if (na != null)
            {
                if (na.arraySize > mapItem.refIndex)
                {
                    return na.GetArrayElementAtIndex(mapItem.refIndex);
                }
            }
            return null;
        }
        SerializedProperty GetNodeArrayProperty(int typeCode)
        {
            foreach (var item in nodeSet.GetType().GetFields())
            {
                var v = item.GetValue(nodeSet) as MGNodeSet.NodeCollection;
                if (v != null)
                {
                    if (v.TypeCode == typeCode)
                    {
                        var prop = nodeSetProperty.FindPropertyRelative(item.Name).FindPropertyRelative("nodes");
                        return prop;
                    }
                }

            }
            return null;
        }

        public void DoLayout()
        {
            if (nodeSetProperty.isExpanded)
            {
                rList.DoLayoutList();
            }
            else
            {
                EditorGUILayout.PropertyField(nodeSetProperty, false);
            }
        }
    }
}
#endif