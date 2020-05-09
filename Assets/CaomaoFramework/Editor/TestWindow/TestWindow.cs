using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using Object = UnityEngine.Object;

namespace UnityEditor
{
    internal class OtherTestWindow : OdinEditorWindow
    {
        private static OtherTestWindow window;
        private bool m_PackableListExpanded;
        private GUIContent packableListLabel;
        private GUIContent testLabel;
        private ReorderableList m_PackableList;
        private List<Object> m_paths = new List<Object>();
        private Object someType;

        [MenuItem("CaomaoTools/Test窗口")]
        public static void OpenWindow()
        {
            window = GetWindow<OtherTestWindow>();
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(800, 600);
        }

        protected override void Initialize()
        {
            base.Initialize();
            this.packableListLabel = new GUIContent("对象拾取器");
            this.testLabel = new GUIContent("测试");
            this.m_PackableList = new ReorderableList(this.m_paths, typeof(string), true, false, true, true);
            this.m_PackableList.drawElementCallback = this.DrawElement;
            this.m_PackableList.onAddCallback = this.AddPackable;
            this.m_PackableList.onRemoveCallback = this.RemovePackable;
            this.m_PackableList.onSelectCallback = this.OnSelectPackable;

            this.m_PackableList.elementHeight = EditorGUIUtility.singleLineHeight;
        }


        private void RemovePackable(ReorderableList list)
        {
            Debug.Log("RemovePackable");
            this.m_paths.RemoveAt(list.index);
        }

        private void OnSelectPackable(ReorderableList list)
        {
            Debug.Log("OnSelect:"+list.index);
        }


        private void DrawElement(Rect rect, int index, bool selected, bool focused)
        {
            Debug.Log("DrawElement");
            //var property = m_Packables.GetArrayElementAtIndex(index);
            var controlID = EditorGUIUtility.GetControlID(-1, FocusType.Passive);
            //var previousObject = property.objectReferenceValue;
            var previousObject = this.m_paths[index];
            var changedObject = EditorGUI.ObjectField(rect, previousObject, typeof(Object), false);
            if (changedObject != previousObject)
            {
                if (previousObject != null)
                {
                    Debug.Log(previousObject.name);
                }
            }

            this.m_paths[index] = changedObject;
            //Debug.Log("Controller:"+controlID);
            //Debug.Log("Selected："+selected);
            //Debug.Log("key:"+GUIUtility.keyboardControl);
            if (GUIUtility.keyboardControl == controlID && !selected)
            {
                m_PackableList.index = index;
                Debug.Log("SelectIndex:"+index);
            }
        }



        private void AddPackable(ReorderableList list)
        {
            Debug.Log("AddPackable");

        }



        protected override void OnGUI()
        {
            base.OnGUI();

            this.HandlerPackableEvent();

        }


        private void HandlerPackableEvent()
        {
            var currentEvent = Event.current;
            var userEvent = false;
            Rect rect = EditorGUILayout.GetControlRect();

            var controllId = GUIUtility.GetControlID(0);
            //Debug.Log(controllId);
            switch (currentEvent.type)
            {
                case EventType.MouseDrag:
                    //Debug.Log("Mouse Drag");
                    userEvent = true;
                    break;

                case EventType.DragExited:
                    //Debug.Log("DragExited");
                    HandleUtility.Repaint();
                    break;
                case EventType.DragUpdated:
                    //Debug.Log("Update");
                    if (rect.Contains(currentEvent.mousePosition) && GUI.enabled)
                    {
                        if (DragAndDrop.objectReferences != null && DragAndDrop.objectReferences.Length > 0)
                        {
                            foreach (var obj in DragAndDrop.objectReferences)
                            {
                                Debug.Log(obj.name);
                                DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
                                DragAndDrop.activeControlID = controllId;
                            }
                        }
                    }
                    break;
                case EventType.DragPerform:
                    //Debug.Log("DragPreform");
                    GUI.changed = true;

                    foreach (var obj in DragAndDrop.objectReferences)
                    {
                        this.m_paths.Add(obj);
                    }

                    DragAndDrop.AcceptDrag();
                    DragAndDrop.activeControlID = 0;
                    userEvent = true;
                    break;
                case EventType.ValidateCommand:
                    Debug.Log("Validate:" + currentEvent.commandName);
                    userEvent = true;
                    break;
                case EventType.ExecuteCommand:
                    Debug.Log("ExcuteCommand:" + currentEvent.commandName);
                    userEvent = true;
                    break;
            }

            m_PackableListExpanded = EditorGUI.Foldout(rect, m_PackableListExpanded, this.packableListLabel, true);
            if (userEvent)
            {
                currentEvent.Use();
            }

            if (this.m_PackableListExpanded)
            {
                EditorGUI.indentLevel++;
                this.m_PackableList.DoLayoutList();
                EditorGUI.indentLevel--;
            }
        }
    }
}