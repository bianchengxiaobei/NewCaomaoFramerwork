using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace CaomaoFramework
{
    /// <summary>
    /// 用Toggle实现的TabButton
    /// </summary>
    public class CUITabToggle : Toggle
    {
        [SerializeField]
        protected int m_index;
        [SerializeField]
        protected CUITab m_parent;//父亲的tab节点

        protected bool m_bOrginIsOn = false;

        public void SetIndex(int index)
        {
            this.m_index = index;
        }


        public int Index
        {
            get => this.m_index;
            set => this.m_index = value;
        }

        public CUITab Parent
        {
            get => this.m_parent;
            set => this.m_parent = value;
        }



        protected override void Awake()
        {
            base.Awake();   
            if (Application.isPlaying)
            {
                Debug.Log("Awake");
                if (this.m_parent == null)
                {
                    if (this.transform.parent != null)
                    {
                        this.m_parent = this.transform.parent.GetComponent<CUITab>();
                    }
                    if (this.m_parent == null)
                    {
                        Debug.LogError("TabToggle == null");
                        return;
                    }
                }
                this.m_parent.InitGroup();
                this.group = this.m_parent.ToggleGroup;
                this.m_bOrginIsOn = this.isOn;
                this.onValueChanged.AddListener(this.OnChangeValue);
            }      
        }

        private void OnChangeValue(bool value)
        {
            if (this.m_parent == null)
            {
                Debug.LogError("parent == null");
                return;
            }
            if (value != this.m_bOrginIsOn)
            {
                //说明改变状态了
                this.m_bOrginIsOn = value;
                this.ChangeSpriteState();
                if (value)
                {
                    this.m_parent.ToggleEvent(this.m_index);
                }
            }
        }

        public virtual void ChangeSpriteState()
        {

        }
    }
}


