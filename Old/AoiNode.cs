using System.Collections.Generic;
using System.Numerics;

namespace AOI.Old
{
    public class AoiNode
    {
        public long Id;

        public Vector2 Position;

        public AoiInfo AoiInfo;

        public AoiLink Link;
        
        public AoiNode(){}

        public AoiNode Init(long id, float x, float y)
        {
            Id = id;

            Position = new Vector2(x, y);

            if (AoiInfo.MovesSet == null) AoiInfo.MovesSet = new HashSet<long>();

            if (AoiInfo.MoveOnlySet == null) AoiInfo.MoveOnlySet = new HashSet<long>();

            if (AoiInfo.EntersSet == null) AoiInfo.EntersSet = new HashSet<long>();
            
            if (AoiInfo.LeavesSet == null) AoiInfo.LeavesSet = new HashSet<long>();

            return this;
        }

        public void SetPosition(float x, float y)
        {
            Position.X = x;

            Position.Y = y;
        }

        public void Dispose()
        {
            Id = 0;
            
            Pool<LinkedListNode<AoiNode>>.Return(Link.XNode);
            Pool<LinkedListNode<AoiNode>>.Return(Link.YNode);
            Link.XNode = null;
            Link.YNode = null;
            AoiInfo.MovesSet.Clear();
            AoiInfo.MoveOnlySet.Clear();
            Pool<AoiNode>.Return(this);
        }
    }
}