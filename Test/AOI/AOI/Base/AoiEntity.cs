using System.Collections.Generic;
using System.Linq;

namespace AOI
{
    public sealed class AoiEntity
    {
        public long Key;
        public AoiLinkedListNode X;
        public AoiLinkedListNode Y;
        public HashSet<long> ViewEntity = new HashSet<long>();
        public HashSet<long> ViewEntityBak = new HashSet<long>();
        public IEnumerable<long> Move => ViewEntity.Union(ViewEntityBak);
        public IEnumerable<long> Leave => ViewEntityBak.Except(ViewEntity);
        private bool _isRecycle;
        public AoiEntity Init(long key)
        {
            Key = key;
            _isRecycle = false;
            return this;
        }

        public void Recycle()
        {
            if (_isRecycle) return;
            
            X = null;
            Y = null;
            Key = 0;
            ViewEntity.Clear();
            ViewEntityBak.Clear();
            _isRecycle = true;
        }
    }
}