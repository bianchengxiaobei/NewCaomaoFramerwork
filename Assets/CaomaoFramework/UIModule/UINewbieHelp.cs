using UnityEngine;
using System.Collections;
using CaomaoFramework;
using System;

public class UINewbieHelp : UIBase
{
    public CUIHelpMask CUIHelpMask;
    public UINewbieGlow CUIHelpGlow;
    public UINewbieHelp()
    {
        this.m_bNotDestroy = false;
        this.m_eHideType = EUIHideType.Active;
        this.m_sResName = "NewbieHelpRoot";
    }

    public override void Init()
    {
        
    }

    public override void OnDisable()
    {
        
    }

    public override void OnEnable()
    {
        
    }

    protected override void InitGraphicComponet()
    {
        this.CUIHelpMask = this.m_oRoot.Find("HelpMask").GetComponent<CUIHelpMask>();
        this.CUIHelpGlow = this.m_oRoot.Find("NewbieHelpRoot/GlowEffect").GetComponent<UINewbieGlow>();
    }

    protected override void OnAddListener()
    {
        
    }

    protected override void OnRemoveListener()
    {
        
    }

    protected override void RealseGraphicComponet()
    {
        
    }
    protected override void PreLoadUI(Action onFinished = null)
    {
        if (m_oRoot)
        {
            Debug.LogError("Window Create Error Exist!");
        }

        if (m_sResName == null || m_sResName == "")
        {
            Debug.LogError("Window Create Error ResName is empty!");
        }
        this.LoadUI(onFinished);
    }
    private async void LoadUI(Action callback) 
    {
        var UIObj = await CaomaoDriver.ResourceModule.LoadGameObjectAsyncNoCallback(this.m_sResName);
        if (UIObj != null)
        {
            this.m_oRoot = UIObj.transform;
            this.m_rectTransform = this.m_oRoot.GetComponent<RectTransform>();
            this.m_oRoot.SetParent(CaomaoDriver.UIRoot);
            this.m_rectTransform.sizeDelta = Vector2.zero;
            this.m_oRoot.localPosition = Vector3.zero;
            this.m_oRoot.localRotation = Quaternion.identity;
            this.m_oRoot.localScale = Vector3.one;
            this.m_oRoot.gameObject.SetActive(false);//设置为隐藏
            InitGraphicComponet();
            callback?.Invoke();
        }
        else
        {
            Debug.LogError($"加载UI Prefab失败:{m_sResName}");
        }
    }
}
