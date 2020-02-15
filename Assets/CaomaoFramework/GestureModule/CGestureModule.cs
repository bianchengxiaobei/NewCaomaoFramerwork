using System;
using System.Collections.Generic;


namespace CaomaoFramework
{
    public enum EGestureType
    {
        e_2D,
        e_3D,
        Mixed
    }
    [Module(true)]
    public class CGestureModule : ICGestureModule, IModule
    {
        public ICGestureModule m_oImp;
        public static EGestureType GestureType = EGestureType.e_2D;
        public void Init()
        {
            this.m_oImp.Init();
        }




        public void Update()
        {
            this.m_oImp.Update();
        }
    }
}
