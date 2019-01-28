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

            AoiInfo = new AoiInfo {MovesSet = new HashSet<AoiNode>(), MoveOnlySet = new HashSet<AoiNode>()};

            Link = new AoiLink();

            return this;
        }

        public Vector2 SetPosition(float x, float y)
        {
            Position.X = x;

            Position.Y = y;

            return Position;
        }
    }
}