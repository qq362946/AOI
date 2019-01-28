using System.Collections.Generic;

namespace ETModel.AOI
{
    public struct AoiInfo
    {
        public HashSet<AoiNode> MovesSet;

        public HashSet<AoiNode> MoveOnlySet;

        public HashSet<AoiNode> EntersSet;

        public HashSet<AoiNode> LeavesSet;

        public void Dispose()
        {
            MovesSet?.Clear();

            MoveOnlySet?.Clear();

            EntersSet?.Clear();

            LeavesSet?.Clear();
        }
    }

    public struct AoiLink
    {
        public LinkedListNode<AoiNode> XNode;

        public LinkedListNode<AoiNode> YNode;

        public void Dispose()
        {
            XNode = null;

            YNode = null;
        }
    }
}