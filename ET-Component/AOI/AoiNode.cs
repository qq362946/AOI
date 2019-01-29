using System.Collections.Generic;
using System.Numerics;

namespace ETModel.AOI
{
    public class AoiNode : Component
    {
        public long Id;

        public Vector2 Position;

        public AoiInfo AoiInfo;

        public AoiLink Link;

        public AoiNode Init(long id, float x, float y)
        {
            Id = id;
            
            Position = new Vector2(x, y);

            AoiInfo = new AoiInfo { MovesSet = new HashSet<long>(), MoveOnlySet = new HashSet<long>() };

            Link = new AoiLink();

            return this;
        }

        public Vector2 SetPosition(float x, float y)
        {
            Position.X = x;

            Position.Y = y;

            return Position;
        }

        public override void Dispose()
        {
            if (this.IsDisposed) return;

            AoiInfo.Dispose();

            Link.Dispose();

            Id = 0;
            
            Position = Vector2.Zero;

            base.Dispose();
        }
    }
}