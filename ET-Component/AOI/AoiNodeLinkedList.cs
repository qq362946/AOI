using System;
using System.Collections.Generic;
using System.Linq;

namespace ETModel.AOI
{
    public enum AoiNodeLinkedListType
    {
        XLink = 0,
        YLink = 1
    }

    public class AoiNodeLinkedList : LinkedList<AoiNode>
    {
        private readonly int _skipCount;

        private readonly AoiNodeLinkedListType _linkedListType;

        public AoiNodeLinkedList(int skip, AoiNodeLinkedListType linkedListType)
        {
            _skipCount = skip;

            _linkedListType = linkedListType;
        }

        public void Insert(AoiNode node)
        {
            if (_linkedListType == AoiNodeLinkedListType.XLink)
            {
                InsertX(node);
            }
            else
            {
                InsertY(node);
            }
        }

        #region Insert

        private void InsertX(AoiNode node)
        {
            if (First == null)
            {
                node.Link.XNode = AddFirst(AoiPool.Instance.Fetch<LinkedListNode<AoiNode>>(node).Value);
            }
            else
            {
                var slowCursor = First;

                var skip = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(Count) / Convert.ToDouble(_skipCount)));

                if (Last.Value.Position.X > node.Position.X)
                {
                    for (var i = 0; i < _skipCount; i++)
                    {
                        // 移动快指针

                        var fastCursor = FastCursor(skip, slowCursor?.Value);

                        // 如果快指针的值小于插入的值，把快指针赋给慢指针，当做当前指针。

                        if (fastCursor.Value.Position.X < node.Position.X)
                        {
                            slowCursor = fastCursor;

                            continue;
                        }

                        // 慢指针移动到快指针位置

                        while (slowCursor != null)
                        {
                            if (slowCursor.Value.Position.X >= node.Position.X)
                            {
                                node.Link.XNode = AoiPool.Instance.Fetch<LinkedListNode<AoiNode>>(node);

                                AddBefore(slowCursor,  node.Link.XNode);

                                return;
                            }

                            slowCursor = slowCursor.Next;
                        }
                    }
                }

                if (node.Link.XNode == null)
                {
                    node.Link.XNode = AddLast(AoiPool.Instance.Fetch<LinkedListNode<AoiNode>>(node).Value);
                }
            }
        }

        private void InsertY(AoiNode node)
        {
            if (First == null)
            {
                node.Link.YNode = AddFirst(AoiPool.Instance.Fetch<LinkedListNode<AoiNode>>(node).Value);
            }
            else
            {
                var slowCursor = First;

                var skip = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(Count) / Convert.ToDouble(_skipCount)));

                if (Last.Value.Position.Y > node.Position.Y)
                {
                    for (var i = 0; i < _skipCount; i++)
                    {
                        // 移动快指针

                        var fastCursor = FastCursor(skip, slowCursor?.Value);

                        // 如果快指针的值小于插入的值，把快指针赋给慢指针，当做当前指针。

                        if (fastCursor.Value.Position.Y <= node.Position.Y)
                        {
                            slowCursor = fastCursor;

                            continue;
                        }

                        // 慢指针移动到快指针位置

                        while (slowCursor != null)
                        {
                            if (slowCursor.Value.Position.Y >= node.Position.Y)
                            {
                                node.Link.YNode = AoiPool.Instance.Fetch<LinkedListNode<AoiNode>>(node);

                                AddBefore(slowCursor,  node.Link.YNode);

                                return;
                            }

                            slowCursor = slowCursor.Next;
                        }
                    }
                }

                if (node.Link.YNode == null)
                {
                    node.Link.YNode = AddLast(AoiPool.Instance.Fetch<LinkedListNode<AoiNode>>(node).Value);
                }
            }
        }        

        #endregion

        private LinkedListNode<AoiNode> FastCursor(int skip, AoiNode currentNode)
        {
            var skipLink = currentNode;

            switch (_linkedListType)
            {
                case AoiNodeLinkedListType.XLink:
                {
                    for (var i = 1; i <= skip; i++)
                    {
                        if (skipLink.Link.XNode.Next == null) break;

                        skipLink = skipLink.Link.XNode.Next.Value;
                    }
                
                    return skipLink.Link.XNode;
                }
                case AoiNodeLinkedListType.YLink:
                {
                    for (var i = 1; i <= skip; i++)
                    {
                        if (skipLink.Link.YNode.Next == null) break;

                        skipLink = skipLink.Link.YNode.Next.Value;
                    }
                
                    return skipLink.Link.YNode;
                }
                default:
                    return null;
            }
        }
    }
}