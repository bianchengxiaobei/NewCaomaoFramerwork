using CaomaoFramework;
using UnityEngine.UI;
public class UIRedPoint : UIBase
{
    public CUIRedPointButton bt1;
    public CUIRedPointButton bt2;
    public CUIRedPointButton bt3;
    public CUIRedPointButton bt4;

    public Button bt_test;

    public UIRedPoint() 
    {
        this.m_bNotDestroy = false;
        this.m_eHideType = EUIHideType.Active;
        this.m_sResName = "UIRedPoint";
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
        this.bt1 = this.m_oRoot.Find("bt1").GetComponent<CUISimpleRedPointButton>();
        this.bt2 = this.m_oRoot.Find("bt2").GetComponent<CUISimpleRedPointButton>();
        this.bt3 = this.m_oRoot.Find("bt3").GetComponent<CUISimpleRedPointButton>();
        this.bt4 = this.m_oRoot.Find("bt4").GetComponent<CUISimpleRedPointButton>();

        this.bt_test = this.m_oRoot.Find("bt_test").GetComponent<Button>();
        this.bt_test.onClick.AddListener(this.OnClickTest);

        //CaomaoDriver.RedPointModule.BindUI(this.bt1);
        //CaomaoDriver.RedPointModule.BindUI(this.bt2);
        //CaomaoDriver.RedPointModule.BindUI(this.bt3);
        //CaomaoDriver.RedPointModule.BindUI(this.bt4);
        CaomaoDriver.RedPointModule.BindUI(this.bt1,this.bt2,this.bt3,this.bt4);
    }

    private void OnClickTest() 
    {
        CaomaoDriver.RedPointModule.NotifyRedPoint("2", true);
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
}