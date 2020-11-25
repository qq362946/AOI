using System;
using System.Collections.Generic;

namespace AOI
{
    public sealed class AoiLinkedListNode
    {
        public float Value;
        public AoiEntity Entity;

        public AoiLinkedListNode Left;
        public AoiLinkedListNode Right;
        public AoiLinkedListNode Top;
        public AoiLinkedListNode Down;

        public AoiLinkedListNode Init(
            float v, AoiEntity p,
            AoiLinkedListNode l, AoiLinkedListNode r,
            AoiLinkedListNode t, AoiLinkedListNode d)
        {
            Left = l;
            Right = r;
            Top = t;
            Down = d;
            Value = v;
            Entity = p;
            
            return this;
        }

        public void Recycle()
        {
            if (Entity != null)
            {
                Entity.Recycle();
                Entity = null;
            }

            Left = null;
            Right = null;
            Top = null;
            Down = null;

            AoiPool.Instance.Recycle(this);
        }
    }

    public sealed class AoiLinkedList
    {
        private int _level;
        private AoiLinkedListNode _header;
        private readonly Random _random = new Random();
        private const float Limit = .001f;

        public AoiLinkedListNode Add(float target, AoiEntity entity)
        {
            var rLevel = 1;
            var addNewHeader = false;
            while (rLevel <= _level && _random.Next(2) == 0) ++rLevel;

            if (rLevel > _level)
            {
                _level = rLevel;
                addNewHeader = true;
                _header = AoiPool.Instance.Fetch<AoiLinkedListNode>().Init(target, entity,
                    null, null, null, _header);
            }

            AoiLinkedListNode cur = _header, last = null;

            for (var l = _level; l >= 1; --l)
            {
                while (cur.Right != null && cur.Right.Value < target) cur = cur.Right;

                if (l <= rLevel)
                {
                    var temp = l == rLevel && addNewHeader
                        ? _header
                        : cur.Right = AoiPool.Instance.Fetch<AoiLinkedListNode>().Init(target, entity,
                            cur, cur.Right, null, null);

                    if (last != null)
                    {
                        last.Down = temp;
                        temp.Top = last;
                    }

                    last = temp;

                    if (l == 1)
                    {
                        cur = temp;
                        break;
                    }
                }

                cur = cur.Down;
            }

            return cur;
        }

        public bool TryGetValue(float target, out AoiLinkedListNode node)
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

        public bool Remove(float target, out AoiEntity entity)
        {
            var cur = _header;
            entity = null;
            var seen = false;

            for (var l = _level; l >= 1; --l)
            {
                while (cur.Right != null && cur.Right.Value < target) cur = cur.Right;

                if (cur.Right != null && Math.Abs(cur.Right.Value - target) < Limit)
                {
                    var temp = cur.Right;
                    entity = temp.Entity;
                    cur.Right = cur.Right.Right;
                    temp.Recycle();
                    seen = true;
                }

                cur = cur.Down;
            }

            return seen;
        }

        public void Move(ref AoiLinkedListNode node, ref float target)
        {
            var cur = node;

            #region Left

            if (target > cur.Value)
            {
                if (cur.Right == null || target <= cur.Right.Value)
                {
                    while (cur != null)
                    {
                        cur.Value = target;
                        cur = cur.Top;
                    }

                    return;
                }

                while (cur != null)
                {
                    var moveCur = cur;

                    moveCur.Value = target;

                    if (cur.Left != null || cur.Right != null)
                    {
                        if (cur.Right != null) cur = cur.Right;

                        if (moveCur.Right != null)
                        {
                            moveCur.Right.Left = moveCur.Left;

                            if (moveCur.Left != null)
                            {
                                moveCur.Left.Right = moveCur.Right;
                                moveCur.Left = null;
                            }

                            moveCur.Right = null;
                        }

                        while (cur.Right != null && cur.Right.Value < target) cur = cur.Right;

                        moveCur.Right = cur.Right;
                        moveCur.Left = cur;
                        
                        if (cur.Right != null) cur.Right.Left = moveCur;
                        
                        cur.Right = moveCur;
                    }

                    cur = moveCur.Top;
                }

                return;
            }

            #endregion

            #region Right

            if (cur.Left == null || target >= cur.Left.Value)
            {
                while (cur != null)
                {
                    cur.Value = target;
                    cur = cur.Top;
                }

                return;
            }

            while (cur != null)
            {
                var moveCur = cur;

                moveCur.Value = target;

                if (cur.Left != null || cur.Right != null)
                {
                    if (cur.Left != null) cur = cur.Left;

                    if (moveCur.Left != null)
                    {
                        moveCur.Left.Right = moveCur.Right;

                        if (moveCur.Right != null)
                        {
                            moveCur.Right.Left = moveCur.Left;
                            moveCur.Right = null;
                        }

                        moveCur.Left = null;
                    }

                    while (cur.Left != null && cur.Left.Value > target) cur = cur.Left;

                    moveCur.Left = cur.Left;
                    moveCur.Right = cur;
                    
                    if (cur.Left != null) cur.Left.Right = moveCur;
                    
                    cur.Left = moveCur;
                }

                cur = moveCur.Top;
            }

            #endregion
        }
    }
}