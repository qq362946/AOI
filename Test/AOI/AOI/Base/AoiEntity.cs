using System.Collections.Generic;

namespace AOI
{
    public class AoiEntity
    {
        public long Key;
        public AoiLinkedListNode X;
        public AoiLinkedListNode Y;
        public readonly HashSet<long> ViewEntity = new HashSet<long>();
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
        }
    }
}