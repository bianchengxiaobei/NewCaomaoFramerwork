using UnityEngine;
using UnityEditor;
using System;
public class CaomaoCallbackHeader : CaomaoHeader
{
    private Action callback;
    public CaomaoCallbackHeader(string title,Action callback):base(title)
    {
        this.callback = callback;
    }
    public override void DrawOhterHeader()
    {
        this.callback?.Invoke();
    }
}