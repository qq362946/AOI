using System.Collections.Generic;

namespace AOI
{
    public struct AoiInfo
    {
        public HashSet<long> MovesSet;

        public HashSet<long> MoveOnlySet;

        public HashSet<long> EntersSet;

        public HashSet<long> LeavesSet; 
    }

    public struct AoiLink
    {
        public LinkedListNode<AoiNode> XNode;

        public LinkedListNode<AoiNode> YNode;
    }
}