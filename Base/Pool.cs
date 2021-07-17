using System;
using System.Collections.Generic;

namespace AOI
{
    public static class Pool<T> where T : class
    {
        private static readonly Queue<T> PoolQueue = new Queue<T>();

        public static T Rent()
        {
            return PoolQueue.Count == 0 ? Activator.CreateInstance<T>() : PoolQueue.Dequeue();
        }
        
        public static T Rent(params object[] args)
        {
            return PoolQueue.Count == 0 ? Activator.CreateInstance<T>() : PoolQueue.Dequeue();
        }

        public static int Count => PoolQueue.Count;
        
        public static void Return(T t)
        {
            if (t == null) return;
            
            PoolQueue.Enqueue(t);
        }

        public static void Clear()
        {
            PoolQueue.Clear();
        }
    }
}