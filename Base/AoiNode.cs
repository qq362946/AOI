namespace AOI
{
    public sealed class AoiNode
    {
        public float Value;
        public int Layer;
        public AoiEntity Entity;
    
        public AoiNode Left;
        public AoiNode Right;
        public AoiNode Top;
        public AoiNode Down;
    
        public AoiNode Init(
            int layer,float v, AoiEntity p,
            AoiNode l, AoiNode r,
            AoiNode t, AoiNode d)
        {
            Layer = layer;
            Left = l;
            Right = r;
            Top = t;
            Down = d;
            Value = v;
            Entity = p;
            
            return this;
        }
    
        public void Recycle()
        {
            Entity = null;
            Left = null;
            Right = null;
            Top = null;
            Down = null;
    
            AoiPool.Instance.Recycle(this);
        }
    }
}