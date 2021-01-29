using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace AOI
{
    public sealed class AoiZone
    {
        private readonly AoiLinkedList _xLinks;
        private readonly AoiLinkedList _yLinks;
        private readonly Dictionary<long, AoiEntity> _entityList = new Dictionary<long, AoiEntity>();

        public AoiZone()
        {
            _xLinks = new AoiLinkedList();
            _yLinks = new AoiLinkedList();
        }

        public AoiZone(float xLinksLimit, float yLinksLimit)
        {
            _xLinks = new AoiLinkedList(limit: xLinksLimit);
            _yLinks = new AoiLinkedList(limit: xLinksLimit);
        }

        public AoiZone(int maxLayer, float xLinksLimit, float yLinksLimit)
        {
            _xLinks = new AoiLinkedList(maxLayer, xLinksLimit);
            _yLinks = new AoiLinkedList(maxLayer, yLinksLimit);
        }

        /// <summary>
        /// Count
        /// </summary>
        public int Count => _entityList.Count;

        public AoiEntity this[long key] => !_entityList.TryGetValue(key, out var entity) ? null : entity;

        /// <summary>
        /// Add a new AoiEntity
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="x">X MinValue = -3.402823E+38f</param>
        /// <param name="y">Y MinValue = -3.402823E+38f</param>
        /// <param name="area"></param>
        /// <param name="enter"></param>
        /// <returns></returns>
        public AoiEntity Enter(long key, float x, float y, Vector2 area, out HashSet<long> enter)
        {
            var entity = Enter(key, x, y);
            Refresh(key, area, out enter);
            return entity;
        }

        /// <summary>
        /// Add a new AoiEntity
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="x">X MinValue = -3.402823E+38f</param>
        /// <param name="y">Y MinValue = -3.402823E+38f</param>
        /// <param name="area"></param>
        /// <param name="enter"></param>
        /// <returns></returns>
        public AoiEntity EnterIncludingMyself(long key, float x, float y, Vector2 area, out HashSet<long> enter)
        {
            var entity = Enter(key, x, y);
            RefreshIncludingMyself(key, area, out enter);
            return entity;
        }

        /// <summary>
        /// Add a new AoiEntity
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="x">X MinValue = -3.402823E+38f</param>
        /// <param name="y">Y MinValue = -3.402823E+38f</param>
        /// <returns></returns>
        public AoiEntity Enter(long key, float x, float y)
        {
            if (_entityList.TryGetValue(key, out var entity)) return entity;

            entity = AoiPool.Instance.Fetch<AoiEntity>().Init(key);

            entity.X = _xLinks.Add(x, entity);
            entity.Y = _yLinks.Add(y, entity);

            _entityList.Add(key, entity);
            return entity;
        }

        /// <summary>
        /// Refresh the AoiEntity
        /// </summary>
        /// <param name="key"></param>
        /// <param name="area"></param>
        /// <param name="enter"></param>
        /// <returns></returns>
        public AoiEntity Refresh(long key, Vector2 area, out HashSet<long> enter)
        {
            var entity = Refresh(key, area);
            enter = entity?.ViewEntity;
            return entity;
        }

        /// <summary>
        /// Refresh the AoiEntity
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="x">X MinValue = -3.402823E+38f</param>
        /// <param name="y">Y MinValue = -3.402823E+38f</param>
        /// <param name="area">view</param>
        /// <param name="enter"></param>
        /// <returns></returns>
        public AoiEntity Refresh(long key, float x, float y, Vector2 area, out HashSet<long> enter)
        {
            var entity = Refresh(key, x, y, area);
            enter = entity?.ViewEntity;
            return entity;
        }

        /// <summary>
        /// Refresh the AoiEntity
        /// </summary>
        /// <param name="key"></param>
        /// <param name="area"></param>
        /// <param name="enter"></param>
        /// <returns></returns>
        public AoiEntity RefreshIncludingMyself(long key, Vector2 area, out HashSet<long> enter)
        {
            var entity = Refresh(key, area, out enter);
            enter?.Add(key);
            return entity;
        }

        /// <summary>
        /// Refresh the AoiEntity
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="x">X MinValue = -3.402823E+38f</param>
        /// <param name="y">Y MinValue = -3.402823E+38f</param>
        /// <param name="area">view</param>
        /// <param name="enter"></param>
        /// <returns></returns>
        public AoiEntity RefreshIncludingMyself(long key, float x, float y, Vector2 area, out HashSet<long> enter)
        {
            var entity = Refresh(key, x, y, area, out enter);
            enter?.Add(key);
            return entity;
        }

        /// <summary>
        /// Refresh the AoiEntity
        /// </summary>
        /// <param name="key"></param>
        /// <param name="area"></param>
        /// <returns></returns>
        public AoiEntity Refresh(long key, Vector2 area)
        {
            if (!_entityList.TryGetValue(key, out var entity)) return null;

            Find(entity, ref area);

            return entity;
        }

        /// <summary>
        /// Refresh the AoiEntity
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="x">X MinValue = -3.402823E+38f</param>
        /// <param name="y">Y MinValue = -3.402823E+38f</param>
        /// <param name="area">view</param>
        /// <returns></returns>
        public AoiEntity Refresh(long key, float x, float y, Vector2 area)
        {
            if (!_entityList.TryGetValue(key, out var entity)) return null;

            var isFind = false;

            if (Math.Abs(entity.X.Value - x) > 0)
            {
                isFind = true;
                _xLinks.Move(entity.X, ref x);
            }

            if (Math.Abs(entity.Y.Value - y) > 0)
            {
                isFind = true;
                _yLinks.Move(entity.Y, ref y);
            }

            if (isFind) Find(entity, ref area);
            return entity;
        }

        /// <summary>
        /// Look for nodes in range
        /// </summary>
        /// <param name="node"></param>
        /// <param name="area"></param>
        /// <returns>news entity</returns>
        private void Find(AoiEntity node, ref Vector2 area)
        {
            SwapViewEntity(ref node.ViewEntity, ref node.ViewEntityBak);

            #region xLinks

            for (var i = 0; i < 2; i++)
            {
                var cur = i == 0 ? node.X.Right : node.X.Left;

                while (cur != null)
                {
                    if (Math.Abs(Math.Abs(cur.Value) - Math.Abs(node.X.Value)) > area.X)
                    {
                        break;
                    }

                    if (Math.Abs(Math.Abs(cur.Entity.Y.Value) - Math.Abs(node.Y.Value)) <= area.Y)
                    {
                        if (Distance(
                            new Vector2(node.X.Value, node.Y.Value),
                            new Vector2(cur.Entity.X.Value, cur.Entity.Y.Value)) <= area.X)
                        {
                            node.ViewEntity.Add(cur.Entity.Key);
                        }
                    }

                    cur = i == 0 ? cur.Right : cur.Left;
                }
            }

            #endregion

            #region yLinks

            for (var i = 0; i < 2; i++)
            {
                var cur = i == 0 ? node.Y.Right : node.Y.Left;

                while (cur != null)
                {
                    if (Math.Abs(Math.Abs(cur.Value) - Math.Abs(node.Y.Value)) > area.Y)
                    {
                        break;
                    }

                    if (Math.Abs(Math.Abs(cur.Entity.X.Value) - Math.Abs(node.X.Value)) <= area.X)
                    {
                        if (Distance(
                            new Vector2(node.X.Value, node.Y.Value),
                            new Vector2(cur.Entity.X.Value, cur.Entity.Y.Value)) <= area.X)
                        {
                            node.ViewEntity.Add(cur.Entity.Key);
                        }
                    }

                    cur = i == 0 ? cur.Right : cur.Left;
                }
            }

            #endregion
        }

        /// <summary>
        /// Exit the AoiZone
        /// </summary>
        /// <param name="key"></param>
        public void Exit(long key)
        {
            if (!_entityList.TryGetValue(key, out var entity)) return;

            Exit(entity);
        }

        /// <summary>
        /// Exit the AoiZone
        /// </summary>
        /// <param name="key"></param>
        /// <param name="node"></param>
        public void Exit(AoiEntity node)
        {
            _xLinks.Remove(node.Key, node.X.Value);
            _yLinks.Remove(node.Key, node.Y.Value);
            _entityList.Remove(node.Key);
            node.Recycle();
        }

        /// <summary>
        /// SwapViewEntity
        /// </summary>
        /// <param name="viewEntity"></param>
        /// <param name="viewEntityBak"></param>
        private static void SwapViewEntity(
            ref HashSet<long> viewEntity,
            ref HashSet<long> viewEntityBak)
        {
            viewEntityBak.Clear();
            var t3 = viewEntity;
            viewEntity = viewEntityBak;
            viewEntityBak = t3;
        }

        /// <summary>
        /// Distance
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        private double Distance(Vector2 a, Vector2 b)
        {
            return Math.Pow((a.X - b.X) * (a.X - b.X) + (a.Y - b.Y) * (a.Y - b.Y), 0.5);
        }
    }
}