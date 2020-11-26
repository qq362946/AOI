using System.Collections.Generic;
using System.Linq;

namespace AOI
{
    public class AoiEntity
    {
        public long Key;
        public AoiLinkedListNode X;
        public AoiLinkedListNode Y;
        public HashSet<long> ViewEntity = new HashSet<long>();
        public HashSet<long> ViewEntityBak = new HashSet<long>();
        public IEnumerable<long> Enter => ViewEntity.Except(ViewEntityBak);
        public IEnumerable<long> Leave => ViewEntityBak.Except(ViewEntity);
        public AoiEntity Init(long key)
        {
            Key = key;

            return this;
        }

        public void Recycle()
        {
            X = null;
            Y = null;
            Key = 0;
            ViewEntity.Clear();
            ViewEntityBak.Clear();
        }
    }
}