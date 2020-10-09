using System;
using System.Collections.Generic;
using UnityEngine;

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
            DeviceInfo.UnitMultiplier = Screen.dpi;
#if UNITY_EDITOR
            this.m_oImp = new PCOldInputGestureImp();
#elif UNITY_ANDROID
            this.m_oImp = new MobileInputGestureImp();
#endif

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
