using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using Sirenix.OdinInspector.Editor;
public class CaomaoPlayerPrefsOptionSeletor : OdinSelector<int>
{
    protected override void BuildSelectionTree(OdinMenuTree tree)
    {
        tree.Config.DrawSearchToolbar = false;
        tree.Selection.SupportsMultiSelect = false;
        tree.Add("新建", 1);
    }
}