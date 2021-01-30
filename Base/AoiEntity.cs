using System.Collections.Generic;
using System.Linq;

namespace AOI
{
    public sealed class AoiEntity
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
            ViewEntity = new HashSet<long>();
            ViewEntityBak = new HashSet<long>();
        }
    }
}