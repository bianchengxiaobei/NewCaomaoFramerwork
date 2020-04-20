using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
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

    public void BeginDrawGroup()
    {
        GUILayout.BeginHorizontal(EditorStyles.toolbar);
    }
    public void EndDrawGruop()
    {
        GUILayout.EndHorizontal();
    }
    public void DrawSingleGroup(string group)
    {
        List<CaomaoToolbarMenuItem> list = null;
        if (this.items.TryGetValue(group,out list))
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

