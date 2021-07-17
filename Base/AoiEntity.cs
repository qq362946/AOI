using System;
using System.Collections.Generic;
using System.Linq;

namespace AOI
{
    public sealed class AoiEntity : IDisposable
    {
        public readonly long Key;
        public AoiNode X;
        public AoiNode Y;
        public HashSet<long> ViewEntity;
        public HashSet<long> ViewEntityBak;
        public IEnumerable<long> Move => ViewEntity.Union(ViewEntityBak);
        public IEnumerable<long> Leave => ViewEntityBak.Except(ViewEntity);

        public AoiEntity(long key)
        {
            Key = key;
            ViewEntity = Pool<HashSet<long>>.Rent();
            ViewEntityBak = Pool<HashSet<long>>.Rent();
        }

        public void Dispose()
        {
            ViewEntity.Clear();
            Pool<HashSet<long>>.Return(ViewEntity);
            ViewEntityBak.Clear();
            Pool<HashSet<long>>.Return(ViewEntityBak);
        }
    }
}