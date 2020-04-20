using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace CaomaoFramework
{
    [AddComponentMenu("CaomaoFramework/CUINumberRedPointButton")]
    [RequireComponent(typeof(CanvasRenderer), typeof(RectTransform), typeof(Image))]
    public class CUINumberRedPointButton : CUIRedPointButton
    {
        public Text lb_number;
        public override IRedPointData BindData(IRedPointData data)
        {
            if (data != null)
            {
                if (data is NumberRedPointData)
                {
                    this.m_oRedPointData = data;
                    this.m_oRedPointData.RegisterUIEvent(this.UpdateView);
                }
                else
                {
                    Debug.LogError("绑定的数据不是NumberRedPointData:" + data.GetType());
                }
            }
            else
            {
                this.m_oRedPointData = new NumberRedPointData(this.ID);
            }
            return this.m_oRedPointData;
        }
        public override void UpdateView()
        {
            base.UpdateView();
            if (this.IsActive() == false) 
            {
                if (this.m_oRedPointData != null)
                {
                    this.lb_number.text = this.m_oRedPointData.GetData();
                }
            }           
        }
    }
}
