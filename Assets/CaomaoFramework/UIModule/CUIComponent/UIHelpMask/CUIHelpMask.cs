using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class CUIHelpMask : Image
{
    private RectTransform m_oValidArea;//有效点击区域
    public override bool IsRaycastLocationValid(Vector2 screenPoint, Camera eventCamera)
    {
        var contain = RectTransformUtility.RectangleContainsScreenPoint
            (this.m_oValidArea, screenPoint, eventCamera);
        return !contain;
    }

    public void SetVaildArea(RectTransform area) 
    {
        this.m_oValidArea = area;
    }
}
