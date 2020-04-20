using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace CaomaoFramework
{
    [AddComponentMenu("CaomaoFramework/CUISimpleRedPointButton")]
    [RequireComponent(typeof(CanvasRenderer),typeof(RectTransform),typeof(Image))]

    public class CUISimpleRedPointButton : CUIRedPointButton
    {
        public override IRedPointData BindData(IRedPointData data)
        {
            if (data != null)
            {
                this.m_oRedPointData = data;
                this.m_oRedPointData.RegisterUIEvent(this.UpdateView);
            }
            else
            {
                this.m_oRedPointData = new SimpleRedPointData(this.ID);
            }
            return this.m_oRedPointData;
        }
    }
}
