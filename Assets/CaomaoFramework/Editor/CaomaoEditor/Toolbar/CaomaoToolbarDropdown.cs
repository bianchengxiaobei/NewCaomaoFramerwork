using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


/// <summary>
/// 窗体上面的工具条
/// </summary>
///用法:
/// var toolbar = new CaomaoToolbarDropdown();
/// var menuItem = new CaomaoToolbarMenuItem("MenuName");
/// menuItem.AddMenuItem(MenumItemName,Callback);//1：菜单单元名字,2:Callback(点击菜单单元执行的方法)
/// toolbar.AddToolbarMenuItems("GroupName",10,menuItem);//1:GroupName,:Space,3:MenuItem(菜单)
/// 
///
///
///总体画是放在OnGUI中:
/// CaomaoToolbarDropdown.BeginDrawGroup();
/// toobar.DrawSingleGroup("GroupName");//画单个Group的菜单
/// GUILayout.FlexibleSpace();      //中间分割（左右两边对齐）      
/// toobar.DrawSingleGroup("GroupName");//画单个Group的菜单
///
///
///
///
///
///
///
///
///
///
/// 
public class CaomaoToolbarDropdown
{
    public Dictionary<string, List<CaomaoToolbarMenuItem>> items = new Dictionary<string, List<CaomaoToolbarMenuItem>>();
    public Dictionary<string, int> spaces = new Dictionary<string, int>();
    private bool m_bFirstDraw = false;
    public void AddToolbarMenuItems(string group,int space,CaomaoToolbarMenuItem item)
    {
        if (this.items.ContainsKey(group))
        {
            this.items[group].Add(item);
            this.spaces[group] = space;
        }
        else
        {
            this.items.Add(group, new List<CaomaoToolbarMenuItem>());
            this.items[group].Add(item);
            this.spaces.Add(group, space);
        }
    }

    public void Draw()
    {
        if (this.items.Count == 0)
        {
            return;
        }
        GUILayout.BeginHorizontal(EditorStyles.toolbar);
        foreach (var g in this.items)
        {
            var groupKey = g.Key;
            foreach (var v in g.Value)
            {
                v.DrawMenuItem();
            }
            GUILayout.Space(this.spaces[groupKey]);
        }
        GUILayout.EndHorizontal();
    }

    //public void BeginDrawGroup()
    //{
    //    GUILayout.BeginHorizontal(EditorStyles.toolbar);
    //}

    public static void BeginDrawGroup()
    {
        GUILayout.BeginHorizontal(EditorStyles.toolbar);
    }

    public static void EndDrawGroup()
    {
        GUILayout.EndHorizontal();
    }

    //public void EndDrawGruop()
    //{
    //    GUILayout.EndHorizontal();
    //}
    public void DrawSingleGroup(string group)
    {
        if (this.items.TryGetValue(group,out var list))
        {
            if (list.Count > 0)
            {
                foreach (var l in list)
                {
                    l.DrawMenuItem();
                }
            }
        }
    }
}

