using UnityEngine;
using System.Collections.Generic;
using System;
namespace CaomaoFramework 
{
    public class ObjectPool<T> where T : new()
    {
        private readonly Stack<T> m_Stack = new Stack<T>();
        private Action<T> m_actionRelease;
        public int countAll { get; private set; }
        public int countActive { get { return countAll - countInactive; } }
        public int countInactive { get { return m_Stack.Count; } }

        public ObjectPool(Action<T> _release)
        {
            this.m_actionRelease = _release;
        }


        public T Get()
        {
            T element;
            if (m_Stack.Count == 0)
            {
                element = new T();
                countAll++;
            }
            else
            {
                element = m_Stack.Pop();
            }
            return element;
        }

        public void Release(T element)
        {
            if (m_Stack.Count > 0 && ReferenceEquals(m_Stack.Peek(), element))
                Debug.LogError("Internal error. Trying to destroy object that is already released to pool.");
            this.m_actionRelease?.Invoke(element);
            m_Stack.Push(element);
        }
    }
}