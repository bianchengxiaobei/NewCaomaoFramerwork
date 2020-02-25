using System;
using ILRuntime.CLR.Method;
using ILRuntime.Runtime.Enviorment;
using ILRuntime.Runtime.Intepreter;
using CaomaoFramework;

public class ClientStateBaseAdapter : CrossBindingAdaptor
{
    public override Type BaseCLRType
    {
        get
        {
            return typeof(ClientStateBase);//这是你想继承的那个类
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
    class Adaptor : ClientStateBase, CrossBindingAdaptorType
    {
        ILTypeInstance instance;
        ILRuntime.Runtime.Enviorment.AppDomain appdomain;
        private IMethod m_enter;
        private IMethod m_leave;

        public Adaptor()
        {

        }

        public Adaptor(ILRuntime.Runtime.Enviorment.AppDomain appdomain, ILTypeInstance instance)
        {
            this.appdomain = appdomain;
            this.instance = instance;
        }

        public ILTypeInstance ILInstance { get { return instance; } }

        public override void OnEnter()
        {
            if (this.m_enter == null)
            {
                this.m_enter = this.instance.Type.GetMethod("OnEnter", 0);
            }
            this.appdomain.Invoke(this.m_enter, this.instance);
        }

        public override void OnLeave()
        {
            if (this.m_leave == null)
            {
                this.m_leave = this.instance.Type.GetMethod("OnLeave", 0);
            }
            this.appdomain.Invoke(this.m_leave, this.instance);
        }
    }
}
