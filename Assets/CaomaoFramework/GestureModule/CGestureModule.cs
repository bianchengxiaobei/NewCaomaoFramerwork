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
    public class CGestureModule : ICGestureModule
    {
        public ICGestureModule m_oImp;
        public static EGestureType GestureType = EGestureType.e_2D;
        public void Init()
        {
            //this.m_oImp = new MobileInputGestureImp();
            this.m_oImp = new PCOldInputGestureImp();
            this.m_oImp.Init();
        }

        public void AddGesture(IGestureActionCallbackBase callback)
        {
            if (callback != null)
            {
                if (this.m_oImp != null)
                {
                    this.m_oImp.AddGesture(callback);
                }
            }
        }


        public void RemoveGesture(IGestureActionCallbackBase callback)
        {
            if (callback != null)
            {
                if (this.m_oImp != null)
                {
                    this.m_oImp.RemoveGesture(callback);
                }
            }
        }


        public void Update()
        {
            this.m_oImp.Update();
        }
    }
}
