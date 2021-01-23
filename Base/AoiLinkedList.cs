using System;

namespace AOI
{
    public sealed class AoiLinkedList
    {
        private int _level;
        private AoiNode _header;
        private readonly Random _random = new Random();
        private const float Limit = .001f;
        public int Count { get; private set; }

        public AoiNode Add(float target, AoiEntity entity)
        {
            var rLevel = 1;
            while (rLevel <= _level && _random.Next(2) == 0) ++rLevel;

            if (rLevel > _level)
            {
                _level = rLevel;
                _header = AoiPool.Instance.Fetch<AoiNode>().Init(_level, target, entity,
                    null, null, null, _header);
            }

            AoiNode cur = _header, last = null;

            for (var l = _level; l >= 1; --l)
            {
                while (cur.Right != null && cur.Right.Value < target) cur = cur.Right;

                if (l <= rLevel)
                {
                    cur.Right = AoiPool.Instance.Fetch<AoiNode>().Init(l, target, entity,
                        cur, cur.Right, null, null);

                    if (last != null)
                    {
                        last.Down = cur.Right;
                        cur.Right.Top = last;
                    }

                    last = cur.Right;

                    if (l == 1)
                    {
                        cur = cur.Right;
                        break;
                    }
                }

                cur = cur.Down;
            }

            Count++;
            return cur;
        }

        public bool TryGetValue(float target, out AoiNode node)
        {
            node = null;

            var cur = _header;

            while (cur != null)
            {
                while (cur.Right != null && cur.Right.Value < target) cur = cur.Right;

                if (cur.Right != null && Math.Abs(cur.Right.Value - target) < Limit)
                {
                    node = cur.Right;
                    while (node.Down != null) node = node.Down;
                    return true;
                }

                cur = cur.Down;
            }

            return false;
        }

        /// <summary>
        /// Remove
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public bool Remove(float target)
        {
            var cur = _header;
            var seen = false;

            for (var l = _level; l >= 1; --l)
            {
                while (cur.Right != null && cur.Right.Value < target) cur = cur.Right;

                if (cur.Right != null && Math.Abs(cur.Right.Value - target) < Limit)
                {
                    var temp = cur.Right;
                    cur.Right = cur.Right.Right;
                    temp.Recycle();
                    seen = true;
                }

                cur = cur.Down;
            }

            Count--;
            return seen;
        }

        /// <summary>
        /// Move
        /// </summary>
        /// <param name="node"></param>
        /// <param name="target"></param>
        public void Move(AoiNode node, ref float target)
        {
            var cur = node;

            #region Left

            if (target > cur.Value)
            {
                while (cur != null)
                {
                    if (cur.Right != null && target > cur.Right.Value)
                    {
                        var findNode = cur;
                        // 寻找到需要移动到的目标节点。
                        while (findNode.Right != null && findNode.Right.Value < target) findNode = findNode.Right;
                        // 熔断当前节点。
                        CircuitBreaker(cur);
                        // 移动到目标节点位置
                        cur.Left = findNode;
                        cur.Right = findNode.Right;
                        if (findNode.Right != null) findNode.Right.Left = cur;
                        findNode.Right = cur;
                    }

                    cur.Value = target;
                    cur = cur.Top;
                }

                return;
            }

            #endregion

            #region Right

            while (cur != null)
            {
                if (cur.Left != null && target < cur.Left.Value)
                {
                    // 寻找到需要移动到的目标节点。
                    var findNode = cur;
                    while (findNode.Left != null && findNode.Left.Value > target) findNode = findNode.Left;
                    // 熔断当前节点。
                    CircuitBreaker(cur);
                    // 移动到目标节点位置。
                    cur.Right = findNode;
                    cur.Left = findNode.Left;
                    if (findNode.Left != null) findNode.Left.Right = cur;
                    findNode.Left = cur;
                }

                cur.Value = target;
                cur = cur.Top;
            }

            #endregion
        }

        /// <summary>
        /// Circuit Breaker
        /// </summary>
        /// <param name="cur"></param>
        private void CircuitBreaker(AoiNode cur)
        {
            if (cur.Left != null) cur.Left.Right = cur.Right;
            if (cur.Right == null) return;
            cur.Right.Left = cur.Left;
            cur.Left = null;
            cur.Right = null;
        }
    }
}