using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CreateAssetMenu(fileName ="GUIStypeSB",menuName = "CaomaoEditorAssets/GUIStypeSB")]
public class GUIStyleSB : ScriptableObject
{
    public GUIStyle GraphBGStyle;
    public GUIStyle NodeHeadStyle;

    [ContextMenu("Lock")]
    public void Lock()
    {
        this.hideFlags = HideFlags.NotEditable;
    }
    [ContextMenu("UnLock")]
    public void UnLock()
    {
        this.hideFlags = HideFlags.None;
    }
}

