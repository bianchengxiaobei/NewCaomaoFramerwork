using UnityEngine;
using System;
using System.Reflection;
namespace CaomaoFramework
{
    public class GameModules
    {
        private IModule[] m_arrayAllUpdateModules = new IModule[11];
        private IModule[] m_arrayAllNoUpdateModules = new IModule[11];
        private int m_iUpdateIndex = 0;
        private int m_iNoUpdateIndex = 0;
        public void Awake()
        {
            foreach (var m in this.m_arrayAllUpdateModules)
            {
                if (m != null)
                {
                    m.Init();
                }               
            }
            foreach (var m in this.m_arrayAllNoUpdateModules)
            {
                if (m != null)
                {
                    m.Init();
                }               
            }
        }
        public void Update()
        {
            foreach (var m in this.m_arrayAllUpdateModules)
            {
                if (m != null)
                {
                    m.Update();
                }              
            }
        }
        public T AddModule<T>() where T : class, IModule
        {
            var ins = Activator.CreateInstance<T>();
            var attr = typeof(T).GetCustomAttribute<ModuleAttribute>();
            if (attr == null || attr.Update)
            {
                this.m_arrayAllUpdateModules[this.m_iUpdateIndex++] = ins;
            }
            else
            {
                this.m_arrayAllNoUpdateModules[this.m_iNoUpdateIndex++] = ins;
            }
            return ins;
        }
        public T GetModule<T>() where T : class, IModule
        {
            foreach (var m in this.m_arrayAllUpdateModules)
            {
                if (m is T)
                {
                    return m as T;
                }
            }
            foreach (var m in this.m_arrayAllNoUpdateModules)
            {
                if (m is T)
                {
                    return m as T;
                }
            }
            Debug.LogError("No Module:" + typeof(T).ToString());
            return null;
        }
    }
}
