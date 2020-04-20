using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace CaomaoFramework
{
    public abstract class CUIRedPointButton : Button
    {
        [SerializeField]
        public string ID;//当前层的id
        //[SerializeField]
        //public List<string> LayerIds = new List<string>();//所在父亲层所有的id(需要在编辑器制作初始化工作)
        [SerializeField]
        public Image RedPointImage;
        [NonSerialized]
        protected IRedPointData m_oRedPointData;


        protected override void Awake()
        {
            base.Awake();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            if (this.m_oRedPointData != null) 
            {
                if (this.RedPointImage != null)
                {
                    this.RedPointImage.gameObject.SetActive(this.m_oRedPointData.bShow);
                }
            }
        }

        public abstract IRedPointData BindData(IRedPointData data);

        /// <summary>
        /// 显示小红点
        /// </summary>
        /// <param name="value"></param>
        public virtual void UpdateView()
        {
            if (this.RedPointImage == null)
            {
                return;
            }
            if (!this.IsActive()) 
            {
                return;
            }
            this.RedPointImage.gameObject.SetActive(this.m_oRedPointData.bShow);
        }

    }
    public enum ERedPointType
    {
        Simple,
        Number
    }
}
