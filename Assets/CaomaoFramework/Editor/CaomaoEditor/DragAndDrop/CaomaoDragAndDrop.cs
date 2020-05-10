using Sirenix.Utilities;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using Object = UnityEngine.Object;
public class CaomaoDragAndDrop
{
    private ReorderableList m_packableList;//显示的list的组件
    private List<Object> m_listObjs = new List<Object>();//存储的数据 

    private bool m_bPackableListExpanded = false;//是否list的是折叠的
    private Action<Object> m_actionChangeListItem;//当改变list的item的时候触发回调
    private Action<int> m_actionSelectItemIndex;//选中list中的item的时候触发回调
    private Action<Object> m_actionSelectItem;//选中list中的item的时候触发回调

    private Func<Object,bool> m_funcFilterObject;//过滤不能选择的item


    private Action<int> m_actionRemoveItemIndex;//移除list中的item的时候触发回调
    private Action<Object> m_actionRemoveItem;//移除list中的item的时候触发回调

    private GUIContent m_labelObjectList;

    private string searchFilter;

    private const float DefaultItemHeight = 18f;
    private bool m_bAddButton = false;
    private int? m_selectItemIndex = null;


    public CaomaoDragAndDrop(string title,Func<Object,bool> funcSearchFilter, string searchFilter = "", bool dragable = true, bool displayHeader = false,bool disAddButton = true,bool disRemoveButton = true,float defaluItemHeight = DefaultItemHeight)
    {
        //初始化list组件
        this.m_packableList = new ReorderableList(this.m_listObjs,typeof(Object),dragable,displayHeader,disAddButton,disRemoveButton);
        this.m_packableList.drawElementCallback = this.DrawElement;
        this.m_packableList.onSelectCallback = this.OnSelectItem;
        this.m_packableList.onAddCallback = this.OnAddItem;
        this.m_packableList.onRemoveCallback = this.OnRemoveItem;
        this.m_packableList.elementHeight = defaluItemHeight;
        this.m_labelObjectList = new GUIContent(title ?? "List对象选择器(可拖拽)");
        this.searchFilter = searchFilter;
        this.m_funcFilterObject = funcSearchFilter;
    }

    private void OnRemoveItem(ReorderableList list)
    {
        var index = list.index;
        if (index >= this.m_listObjs.Count)
        {
            return;
        }
        if (this.m_actionRemoveItem != null)
        {
            var removeItem = this.m_listObjs[index];
            this.m_actionRemoveItem?.Invoke(removeItem);
        }
        this.m_listObjs.RemoveAt(index);
        this.m_actionRemoveItemIndex?.Invoke(index);
    }

    private void OnAddItem(ReorderableList list)
    {
        //出现选中project里面的资源，比如文件夹，图片等等
        //需要在这里过滤
        EditorGUIUtility.ShowObjectPicker<Object>(null, false, this.searchFilter, -1);
        this.m_bAddButton = true;
    }

    /// <summary>
    /// 画每个list里面的元素组件UI
    /// </summary>
    /// <param name="rect"></param>
    /// <param name="index"></param>
    /// <param name="selected"></param>
    /// <param name="focused"></param>
    private void DrawElement(Rect rect, int index, bool selected, bool focused)
    {
        if (index >= this.m_listObjs.Count)
        {
            return;
        }
        var controllId = EditorGUIUtility.GetControlID(-1, FocusType.Passive);
        var previoursObject = this.m_listObjs[index];
        Object changedObject = EditorGUI.ObjectField(rect, previoursObject, typeof(Object), false) as Object;
        if (previoursObject != changedObject)
        {
            if (previoursObject != null)
            {
                this.m_actionChangeListItem?.Invoke(previoursObject);
            }
            this.m_listObjs[index] = changedObject;//改变list的里面item
        }

        if (GUIUtility.keyboardControl == controllId && selected == false)
        {
            //在list列表中选择该item
            this.m_packableList.index = index;
        }
    }

    private void OnSelectItem(ReorderableList list)
    {
        var selectIndex = list.index;
        this.m_selectItemIndex = selectIndex;
        this.m_actionSelectItemIndex?.Invoke(selectIndex);
        if (this.m_actionSelectItem != null)
        {
            if (selectIndex >= this.m_listObjs.Count)
            {
                var selectItem = this.m_listObjs[selectIndex];
                if (selectItem != null)
                {
                    this.m_actionSelectItem.Invoke(selectItem);
                }
            }
        }
    }





    public void SetRemoveItemCallback(Action<Object> callback)
    {
        this.m_actionRemoveItem = callback;
    }

    public void SetRemoveItemCallback(Action<int> callback)
    {
        this.m_actionRemoveItemIndex = callback;
    }

    public void SetSelectItemCallback(Action<Object> callback)
    {
        this.m_actionSelectItem = callback;
    }

    public void SetSelectItemIndexCallback(Action<int> callback)
    {
        this.m_actionSelectItemIndex = callback;
    }
    /// <summary>
    /// 改变list里面item的时候，触发的回调
    /// </summary>
    /// <param name="callback"></param>
    public void SetChangeItemCallback(Action<Object> callback)
    {
        this.m_actionChangeListItem = callback;
    }





    /// <summary>
    /// 显示在Inspector界面里面
    /// </summary>
    public void OnInspectorGUI()
    {
        var currentEvent = Event.current;
        var userEvent = false;
        Rect rect = EditorGUILayout.GetControlRect();
        //Rect rect = EditorGUILayout.BeginVertical();
        //Debug.Log(rect);
        var controllId = GUIUtility.GetControlID(0);
        //Debug.Log(controllId);
        switch (currentEvent.type)
        {
            case EventType.DragExited:
                HandleUtility.Repaint();
                break;
            case EventType.DragUpdated:
            case EventType.DragPerform:
                if (rect.Contains(currentEvent.mousePosition) && GUI.enabled)
                {
                    if (DragAndDrop.objectReferences != null && DragAndDrop.objectReferences.Length > 0)
                    {
                        bool acceptObj = false;
                        foreach (var obj in DragAndDrop.objectReferences)
                        {
                            DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
                            if (currentEvent.type == EventType.DragPerform)
                            {
                                var temp = obj as Object;
                                this.m_listObjs.Add(temp);
                                acceptObj = true;
                                DragAndDrop.activeControlID = 0;
                            }
                            else
                            {
                                DragAndDrop.activeControlID = controllId;
                            }
                        }
                        if (acceptObj)
                        {
                            GUI.enabled = true;
                            DragAndDrop.AcceptDrag();
                            userEvent = true;
                        }
                    }
                }
                break;
            case EventType.ValidateCommand:
                Debug.Log("Validate:" + currentEvent.commandName);
                userEvent = true;
                break;
            case EventType.ExecuteCommand:
                Debug.Log("ExcuteCommand:" + currentEvent.commandName);
                //ExcuteCommand:ObjectSelectorUpdated
                if (currentEvent.commandName == "ObjectSelectorClosed")
                {
                    //选择结束
                    var obj = EditorGUIUtility.GetObjectPickerObject();
                    if (obj != null && this.m_funcFilterObject(obj))
                    {
                        Debug.Log(obj.name);
                        if (this.m_bAddButton)
                        {
                            //说明是添加
                            this.m_bAddButton = false;
                            this.m_listObjs.Add(obj);
                        }
                        else 
                        {
                            //说明是改变
                            if (this.m_selectItemIndex != null)
                            {
                                this.m_listObjs[this.m_selectItemIndex.Value] = obj;
                                this.m_selectItemIndex = null;
                            }
                            else 
                            {
                                this.m_listObjs[0] = obj;
                            }
                        }
                    }
                }

                userEvent = true;
                break;
        }

        if (this.m_labelObjectList != null)
        {
            rect.x = 0;
            rect.y = 0;
            this.m_bPackableListExpanded = EditorGUI.Foldout(rect, this.m_bPackableListExpanded, this.m_labelObjectList, true);
        }
        if (userEvent)
        {
            currentEvent.Use();
        }

        if (this.m_bPackableListExpanded)
        {
            EditorGUI.indentLevel++;
            //this.m_packableList.DoLayoutList();
            var foldRect = rect.AddY(this.m_packableList.headerHeight);
            this.m_packableList.DoList(foldRect);
            EditorGUI.indentLevel--;
        }
        //EditorGUILayout.EndVertical();
    }
}

