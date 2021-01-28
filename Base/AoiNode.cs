namespace AOI
{
    public sealed class AoiNode
    {
        public float Value;
        public int Layer;
        public int Count;
        public AoiEntity Entity;
    
        public AoiNode Left;
        public AoiNode Right;
        public AoiNode Top;
        public AoiNode Down;

        public AoiNode Init(int layer, float v = 0, AoiEntity entity = null, AoiNode left = null, AoiNode right = null,
            AoiNode top = null,
            AoiNode down = null)
        {
            Layer = layer;
            Left = left;
            Right = right;
            Top = top;
            Down = down;
            Value = v;
            Entity = entity;

            return this;
        }

        public void Recycle()
        {
            Value = 0;
            Layer = 0;
            Count = 0;
            Entity = null;
            Left = null;
            Right = null;
            Top = null;
            Down = null;
    
            AoiPool.Instance.Recycle(this);
        }
    }
}