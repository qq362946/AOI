namespace AOI
{
    public class SkipListNode<T>
    {
        public long Value;
        public T Obj;
        public SkipListNode<T> Right;
        public SkipListNode<T> Down;

        public SkipListNode<T> Init(long v, T o, SkipListNode<T> r, SkipListNode<T> d)
        {
            Right = r;
            Down = d;
            Value = v;
            Obj = o;

            return this;
        }
    }
}