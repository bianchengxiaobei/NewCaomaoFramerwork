using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace CaomaoFramework
{
    /// <summary>
    /// Tab封装，内部使用的是ToggleGroup来实现
    /// </summary>
    [RequireComponent(typeof(ToggleGroup))]
    public class CUITab : MonoBehaviour
    {
        private CUITabToggle[] m_toggles;//在tab下面所有的toggle

        private ToggleGroup m_toggleGroup;

        private Dictionary<int, CUITabToggle> m_dicTabToggles = new Dictionary<int, CUITabToggle>();


        private Action<int> onClick;

        public ToggleGroup ToggleGroup
        {
            get => this.m_toggleGroup;
        }



        private void Awake()
        {
            this.InitGroup();
            if (this.m_toggles == null || this.m_toggles.Length == 0)
            {
                this.m_toggles = this.transform.GetComponentsInChildren<CUITabToggle>();

                if (this.m_toggles == null || this.m_toggles.Length == 0)
                {
                    Debug.LogError("Toggles == null || Toggles.Length == 0");
                }

                foreach (var tabButton in this.m_toggles)
                {
                    this.m_dicTabToggles.Add(tabButton.Index, tabButton);
                }
            }
        }

        private void Start()
        {
            //默认选择第一个
            //if (this.m_toggles.Length > 0)
            //{
            //    this.m_toggles[0].isOn = true;
            //}
        }
        /// <summary>
        /// 第一次进入的时候选择一个默认的TabButton
        /// </summary>
        /// <param name="btnIndex"></param>
        public void SelectTabButton(int btnIndex)
        {
            if (this.m_dicTabToggles.TryGetValue(btnIndex, out var tabButton))
            {
                tabButton.isOn = true;
            }
            else
            {
                Debug.LogError("No TabButton:" + btnIndex);
            }
        }
        public void InitGroup()
        {
            if (this.m_toggleGroup == null)
            {
                this.m_toggleGroup = this.GetComponent<ToggleGroup>();
                if (this.m_toggleGroup == null)
                {
                    Debug.LogError("ToggleGroup == null");
                }
            }
        }


        /// <summary>
        /// 注册点击事件
        /// </summary>
        /// <param name="onClick"></param>
        public void RegisterOnClick(Action<int> onClick)
        {
            this.onClick = onClick;
        }
        /// <summary>
        /// 触发点击事件
        /// </summary>
        /// <param name="index"></param>
        public void ToggleEvent(int index)
        {
            this.onClick?.Invoke(index);
        }
    }
}

