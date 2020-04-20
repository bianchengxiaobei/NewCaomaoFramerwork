using UnityEngine;
namespace CaomaoFramework 
{
    public class CaomaoRuntimeGlobalSBConfig<T> : ScriptableObject where T : ScriptableObject
    {
        private static T m_instance;

        public static T Instance
        {
            get
            {
                if (m_instance == null)
                {
                    m_instance = Resources.Load<T>(typeof(T).Name);
                }
                return m_instance;
            }
        }
    }
}
