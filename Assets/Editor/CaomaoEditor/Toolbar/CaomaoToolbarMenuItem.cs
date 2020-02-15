using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
public class CaomaoToolbarMenuItem
{
    private GUIContent buttonName;
    private GenericMenu Menu;
    private string m_sName;
    private int m_iCount = 0;
    public CaomaoToolbarMenuItem(string name)
    {
        this.m_sName = name;
        this.m_iCount = 0;
        this.buttonName = new GUIContent(name);
        this.Menu = new GenericMenu();
    }

    public void AddMenuItem(string name, GenericMenu.MenuFunction callback)
    {
        this.Menu.AddItem(new GUIContent(name), false, callback);
        this.m_iCount++;
    }
    public void AddMenuItem(GUIContent nameGui, GenericMenu.MenuFunction callback)
    {
        this.Menu.AddItem(nameGui, false, callback);
        this.m_iCount++;
    }
    public virtual void DrawMenuItem()
    {
        if (string.IsNullOrEmpty(this.m_sName) || this.m_iCount == 0)
        {
            Debug.LogError($"No Name:{this.m_sName} or Count={this.m_iCount}");
            return;
        }
        if (GUILayout.Button(this.buttonName, EditorStyles.toolbarDropDown, GUILayout.Width(50)))
        {
            this.Menu.ShowAsContext();
        }       
    }
}

