using System;
using ILRuntime.CLR.Method;
using ILRuntime.Runtime.Enviorment;
using ILRuntime.Runtime.Intepreter;
using CaomaoFramework;
using UnityEngine;
public class TestApadater : CrossBindingAdaptor
{
    public override Type BaseCLRType
    {
        get
        {
            return typeof(ITestA);//这是你想继承的那个类
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
    class Adaptor : ITestA, CrossBindingAdaptorType
    {
        ILTypeInstance instance;
        ILRuntime.Runtime.Enviorment.AppDomain appdomain;
        private IMethod m_start;




        public Adaptor()
        {

        }

        public Adaptor(ILRuntime.Runtime.Enviorment.AppDomain appdomain, ILTypeInstance instance)
        {
            this.appdomain = appdomain;
            this.instance = instance;
        }

        public ILTypeInstance ILInstance { get { return instance; } }

        public void Start()
        {
            if (this.m_start == null)
            {
                this.m_start = this.instance.Type.GetMethod("Start",0);
                //this.m_start = this.instance.Type.GetMethod("Start");
            }
            if (this.m_start == null)
            {
                Debug.LogError("111");
            }
            else
            {
                Debug.Log(this.m_start.Name);
            }
            this.appdomain.Invoke(this.m_start, this.instance);
        }
    }
}
