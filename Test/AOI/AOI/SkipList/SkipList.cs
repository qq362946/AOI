using System;
using System.Collections.Generic;

namespace AOI
{
    /// <summary>
    /// 跳跃表
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SkipList<T>
    {
        private int _level;
        private SkipListNode<T> _header;
        private readonly Random _random = new Random();

        public void Add(long target, T obj)
        {
            var rLevel = 1;
            while (rLevel <= _level && _random.Next(2) == 0) ++rLevel;

            if (rLevel > _level)
            {
                _level = rLevel;
                _header = new SkipListNode<T>().Init(target, obj, null, _header);
            }

            SkipListNode<T> cur = _header, last = null;

            for (var l = _level; l >= 1; --l)
            {
                while (cur.Right != null && cur.Right.Value < target) cur = cur.Right;

                if (l <= rLevel)
                {
                    cur.Right = new SkipListNode<T>().Init(target, obj, cur.Right, null);

                    if (last != null) last.Down = cur.Right;

                    last = cur.Right;
                }

                cur = cur.Down;
            }
        }

        public bool TryGetValue(long target, out SkipListNode<T> node)
        {
            node = null;

            var cur = _header;

            while (cur != null)
            {
                while (cur.Right != null && cur.Right.Value < target) cur = cur.Right;

                if (cur.Right != null && cur.Right.Value == target)
                {
                    node = cur.Right;
                    while (node.Down != null) node = node.Down;
                    return true;
                }

                cur = cur.Down;
            }

            return false;
        }

        public bool Remove(long target, out T obj)
        {
            var cur = _header;
            obj = default;
            var seen = false;

            for (var l = _level; l >= 1; --l)
            {
                while (cur.Right != null && cur.Right.Value < target) cur = cur.Right;

                if (cur.Right != null && cur.Right.Value == target)
                {
                    obj = cur.Right.Obj;
                    cur.Right = cur.Right.Right;
                    seen = true;
                }

                cur = cur.Down;
            }

            return seen;
        }
    }
}