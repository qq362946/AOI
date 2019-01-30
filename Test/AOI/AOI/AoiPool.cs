using System;
using System.Collections.Generic;

namespace AOI
{
    public class AoiPool
    {
        public static AoiPool Instance => _instance ?? (_instance = new AoiPool());

        private static AoiPool _instance;
        
        private readonly Dictionary<Type, Queue<object>> _dic = new Dictionary<Type, Queue<object>>();

        public T Fetch<T>() where T : class
        {
            var type = typeof(T);

            if (_dic.TryGetValue(type, out var queue))
            {
                return queue.Count > 0 ? (T) queue.Dequeue() : (T) Activator.CreateInstance(type);
            }

            queue = new Queue<object>();

            _dic.Add(type, queue);

            return (T) Activator.CreateInstance(type);
        }

        public T Fetch<T>(params object[] args) where T : class
        {
            var type = typeof(T);

            if (_dic.TryGetValue(type, out var queue))
            {
                return queue.Count > 0 ? (T) queue.Dequeue() : (T) Activator.CreateInstance(type, args);
            }

            queue = new Queue<object>();

            _dic.Add(type, queue);

            return (T) Activator.CreateInstance(type, args);
        }

        public void Recycle(object obj)
        {
            var type = obj.GetType();

            if (!_dic.TryGetValue(type, out var queue))
            {
                queue = new Queue<object>();

                _dic.Add(type, queue);
            }

            queue.Enqueue(obj);
        }
    }
}