using System;
using ILRuntime.CLR.Method;
using ILRuntime.Runtime.Enviorment;
using ILRuntime.Runtime.Intepreter;
using CaomaoFramework;

public class UIBaseApadater : CrossBindingAdaptor
{
    public override Type BaseCLRType
    {
        get
        {
            return typeof(UIBase);//这是你想继承的那个类
        }
    }

    public override Type AdaptorType
    {
        get
        {
            return typeof(Adaptor);//这是实际的适配器类
        }
    }
    public override object CreateCLRInstance(ILRuntime.Runtime.Enviorment.AppDomain appdomain, ILTypeInstance instance)
    {
        return new Adaptor(appdomain, instance);//创建一个新的实例
    }
    //实际的适配器类需要继承你想继承的那个类，并且实现CrossBindingAdaptorType接口
    class Adaptor : UIBase, CrossBindingAdaptorType
    {
        ILTypeInstance instance;
        ILRuntime.Runtime.Enviorment.AppDomain appdomain;
        private IMethod m_init;
        private IMethod m_enable;
        private IMethod m_disable;
        private IMethod m_initGraph;
        private IMethod m_releaseGraph;
        private IMethod m_addLis;
        private IMethod m_removeLis;


        private IMethod m_createUI;
        private IMethod m_preloadUI;
        private bool m_bCreateUIInvoke;
        private bool m_bPreloadUIInvoke;
        private IMethod m_update;
        private bool m_bUpdateInvoke;
        private IMethod m_goToForwardLayer;
        private bool m_bGotoForwardLayer;
       



        public Adaptor()
        {

        }

        public Adaptor(ILRuntime.Runtime.Enviorment.AppDomain appdomain, ILTypeInstance instance)
        {
            this.appdomain = appdomain;
            this.instance = instance;
        }

        public ILTypeInstance ILInstance { get { return instance; } }

        protected override void CreateUI()
        {
            if (this.m_createUI == null)
            {
                this.m_createUI = instance.Type.GetMethod("CreateUI", 0);
            }
            if (this.m_createUI != null && this.m_bCreateUIInvoke == false)
            {
                this.m_bCreateUIInvoke = true;
                this.appdomain.Invoke(this.m_createUI, this.instance, null);
                this.m_bCreateUIInvoke = false;
            }
            else
            {
                base.CreateUI();
            }            
        }

        protected override void PreLoadUI()
        {
            if (this.m_preloadUI == null)
            {
                this.m_preloadUI = instance.Type.GetMethod("PreLoadUI", 0);
            }
            if (this.m_preloadUI != null && this.m_bPreloadUIInvoke == false)
            {
                this.m_bPreloadUIInvoke = true;
                this.appdomain.Invoke(this.m_preloadUI, this.instance, null);
                this.m_bPreloadUIInvoke = false;
            }
            else
            {
                base.PreLoadUI();
            }
        }

        public override void Update()
        {
            if (this.m_update == null)
            {
                this.m_update = instance.Type.GetMethod("Update", 0);
            }
            if (this.m_update != null && this.m_bUpdateInvoke == false)
            {
                this.m_bUpdateInvoke = true;
                this.appdomain.Invoke(this.m_update, this.instance);
                this.m_bUpdateInvoke = false;
            }
            else
            {
                base.Update();
            }          
        }

        protected override void GoToForwardLayer()
        {
            if (this.m_goToForwardLayer == null)
            {
                this.m_goToForwardLayer = instance.Type.GetMethod("GoToForwardLayer", 0);
            }
            if (this.m_goToForwardLayer != null && this.m_bGotoForwardLayer == false)
            {
                this.m_bGotoForwardLayer = true;
                this.appdomain.Invoke(this.m_goToForwardLayer, this.instance);
                this.m_bGotoForwardLayer = false;
            }
            else
            {
                base.GoToForwardLayer();
            }          
        }

        public override void Init()
        {
            if (this.m_init == null)
            {
                this.m_init = instance.Type.GetMethod("Init", 0);
            }
            this.appdomain.Invoke(this.m_init,this.instance,null);
        }

        public override void OnDisable()
        {
            if (this.m_disable == null)
            {
                this.m_disable = instance.Type.GetMethod("OnDisable", 0);
            }
            this.appdomain.Invoke(this.m_disable, this.instance, null);
        }

        public override void OnEnable()
        {
            if (this.m_enable == null)
            {
                this.m_enable = instance.Type.GetMethod("OnEnable", 0);
            }
            this.appdomain.Invoke(this.m_enable, this.instance, null);
        }

        public override string ToString()
        {
            IMethod m = appdomain.ObjectType.GetMethod("ToString", 0);
            m = instance.Type.GetVirtualMethod(m);
            if (m == null || m is ILMethod)
            {
                return instance.ToString();
            }
            else
                return instance.Type.FullName;
        }

        protected override void InitGraphicComponet()
        {
            if (this.m_initGraph == null)
            {
                this.m_initGraph = instance.Type.GetMethod("InitGraphicComponet", 0);
            }
            this.appdomain.Invoke(this.m_initGraph, this.instance, null);
        }

        protected override void OnAddListener()
        {
            if (this.m_addLis == null)
            {
                this.m_addLis = instance.Type.GetMethod("OnAddListener", 0);
            }
            this.appdomain.Invoke(this.m_addLis, this.instance, null);
        }

        protected override void OnRemoveListener()
        {
            if (this.m_removeLis == null)
            {
                this.m_removeLis = instance.Type.GetMethod("OnRemoveListener", 0);
            }
            this.appdomain.Invoke(this.m_removeLis, this.instance, null);
        }

        protected override void RealseGraphicComponet()
        {
            if (this.m_releaseGraph == null)
            {
                this.m_releaseGraph = instance.Type.GetMethod("RealseGraphicComponet", 0);
            }
            this.appdomain.Invoke(this.m_releaseGraph, this.instance, null);
        }
    }
}
