using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace CaomaoFramework
{
    public abstract class CUIRedPointButton : Button
    {
        [SerializeField]
        public string ID;
        [HideInInspector]
        public List<string> LayerIds = new List<string>();
        [SerializeField]
        private Image RedPointImage;
        [NonSerialized]
        protected IRedPointData m_oRedPointData;


        protected override void Awake()
        {
            base.Awake();
            if (this.RedPointImage != null)
            {
                this.RedPointImage.gameObject.SetActive(false);
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
            this.RedPointImage.gameObject.SetActive(this.m_oRedPointData.bShow);
        }

    }
    public enum ERedPointType
    {
        Simple,
        Number
    }
}
