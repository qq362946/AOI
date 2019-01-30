using System.Collections.Generic;
using System.Numerics;

namespace AOI
{
    public class AoiNode
    {
        public long Id;

        public Vector2 Position;

        public AoiInfo AoiInfo;

        public AoiLink Link;

        public AoiNode Init(long id, float x, float y)
        {
            Id = id;

            Position = new Vector2(x, y);

            if (AoiInfo.MovesSet == null)
            {
                AoiInfo.MovesSet = new HashSet<long>();
            }

            if (AoiInfo.MoveOnlySet == null)
            {
                AoiInfo.MoveOnlySet = new HashSet<long>();
            }

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
            
            Link.XNode = null;

            Link.YNode = null;
            
            AoiPool.Instance.Recycle(Link.XNode);
            
            AoiPool.Instance.Recycle(Link.YNode);
            
            AoiInfo.MovesSet.Clear();
                
            AoiInfo.MoveOnlySet.Clear();
            
            AoiPool.Instance.Recycle(this);
        }
    }
}