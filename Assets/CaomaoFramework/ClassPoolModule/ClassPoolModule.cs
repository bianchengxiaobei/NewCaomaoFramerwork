using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
namespace CaomaoFramework
{
    public static class ClassPoolModule<T> where T : class, IClassInstance, new()
    {
        private static T[] m_activeObjects;
        private static Stack<T> m_recyleObjects;
        private static bool m_Init = false;
        public static void Init()
        {
            if (m_Init)
            {
                return;
            }
            m_Init = true;
            Type typeFromHandle = typeof(T);
            object[] customAttributes = typeFromHandle.GetCustomAttributes(typeof(CInstanceNumber), true);
            if (customAttributes != null && customAttributes.Length != 0)
            {
                CInstanceNumber aInstanceNumber = customAttributes[0] as CInstanceNumber;
                m_activeObjects = new T[aInstanceNumber.Num];
                m_recyleObjects = new Stack<T>(aInstanceNumber.Num);
            }
        }
        /// <summary>
        /// 线程安全创建实例
        /// </summary>
        /// <returns></returns>
        public static T SafeAlloc()
        {
            Init();
            T t = default(T);
            Monitor.Enter(m_recyleObjects);
            try
            {
                t = m_recyleObjects.Count > 0 ? m_recyleObjects.Pop() : Activator.CreateInstance<T>();
            }
            finally
            {
                Monitor.Exit(m_recyleObjects);
            }
            Monitor.Enter(m_activeObjects);
            try
            {
                for (int i = 0; i < m_activeObjects.Length; i++)
                {
                    if (m_activeObjects[i] == null)
                    {
                        m_activeObjects[i] = t;
                        break;
                    }
                }
            }
            finally
            {
                Monitor.Exit(m_activeObjects);
            }
            t.OnAlloc();
            return t;
        }
        /// <summary>
        /// 非线程安全创建实例
        /// </summary>
        /// <returns></returns>
        public static T Alloc()
        {
            Init();
            var t = m_recyleObjects.Count > 0 ? m_recyleObjects.Pop() : Activator.CreateInstance<T>();
            for (int i = 0; i < m_activeObjects.Length; i++)
            {
                if (m_activeObjects[i] == null)
                {
                    m_activeObjects[i] = t;
                    break;
                }
            }
            t.OnAlloc();
            return t;
        }
        /// <summary>
        /// 线程安全回收实例
        /// </summary>
        /// <param name="obj"></param>
        public static void SafeRelease(T obj)
        {
            if (null != obj)
            {
                obj.OnRelease();
                Monitor.Enter(m_activeObjects);
                try
                {
                    for (int i = 0; i < m_activeObjects.Length; i++)
                    {
                        if (object.ReferenceEquals(m_activeObjects[i], obj))
                        {
                            m_activeObjects[i] = null;
                            Monitor.Enter(m_recyleObjects);
                            try
                            {
                                m_recyleObjects.Push(obj);
                            }
                            catch (Exception e)
                            {
                                Debug.LogException(e);
                            }
                            finally
                            {
                                Monitor.Exit(m_recyleObjects);
                            }
                            break;
                        }
                    }
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
                finally
                {
                    Monitor.Exit(m_activeObjects);
                }
            }
        }
        public static void Release(T obj)
        {
            if (null != obj)
            {
                obj.OnRelease();
                for (int i = 0; i < m_activeObjects.Length; i++)
                {
                    if (object.ReferenceEquals(m_activeObjects[i], obj))
                    {
                        m_activeObjects[i] = null;
                        m_recyleObjects.Push(obj);
                        break;
                    }
                }
            }
        }
    }
}
