using System.Collections.Generic;

namespace AOI
{
    public struct AoiInfo
    {
        public HashSet<AoiNode> MovesSet;

        public HashSet<AoiNode> MoveOnlySet;

        public HashSet<AoiNode> EntersSet;

        public HashSet<AoiNode> LeavesSet; 
    }

    public struct AoiLink
    {
        public LinkedListNode<AoiNode> XNode;

        public LinkedListNode<AoiNode> YNode;
    }
}