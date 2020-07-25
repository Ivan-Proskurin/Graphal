using Graphal.Engine.TwoD.Transforms;

namespace Graphal.Engine.Persistence.TwoD.Transforms
{
    public class ShiftTransform2Ds : Transform2Ds
    {
        public int OffsetX { get; set; }

        public int OffsetY { get; set; }

        public override Transform2D ToTransform2D()
        {
            return new ShiftTransform2D(OffsetX, OffsetY);
        }
    }
}