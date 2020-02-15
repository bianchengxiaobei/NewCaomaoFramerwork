using System;
namespace CaomaoFramework
{
    public class Singleton<T> where T : new()
    {
        private static T s_singleton = default(T);

        public static T Instance
        {
            get
            {
                if (null == s_singleton)
                {
                    s_singleton = Activator.CreateInstance<T>();
                }
                return s_singleton;
            }
        }

        protected Singleton()
        {
        }
    }
    public class SafeSingleton<T> where T : new()
    {
        private static T s_singleton = default(T);

        private static object s_objectLock = new object();

        public static T singleton
        {
            get
            {
                if (null == SafeSingleton<T>.s_singleton)
                {
                    lock (SafeSingleton<T>.s_objectLock)
                    {
                        if (null == SafeSingleton<T>.s_singleton)
                        {
                            SafeSingleton<T>.s_singleton = ((default(T) == null) ? Activator.CreateInstance<T>() : default(T));
                        }
                    }
                }
                return SafeSingleton<T>.s_singleton;
            }
        }

        protected SafeSingleton()
        {
        }
    }
}
