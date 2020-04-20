using System.Collections.Generic;
namespace CaomaoFramework 
{
    public static class ListPoolModule<T>
    {
        private static readonly ObjectPool<List<T>> s_ListPool = new ObjectPool<List<T>>(Clear);
        static void Clear(List<T> l)
        { 
            l.Clear(); 
        }

        public static List<T> Alloc()
        {
            return s_ListPool.Get();
        }

        public static void Release(List<T> toRelease)
        {
            s_ListPool.Release(toRelease);
        }
    }
}
