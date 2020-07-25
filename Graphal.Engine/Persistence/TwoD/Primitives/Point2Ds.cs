using Graphal.Engine.TwoD.Primitives;

namespace Graphal.Engine.Persistence.TwoD.Primitives
{
    public class Point2Ds : Primitive2Ds
    {
        public int X { get; set; }

        public int Y { get; set; }

        public int Color { get; set; }

        public override Primitive2D ToPrimitive2D()
        {
            return new Point2D(X, Y, System.Drawing.Color.FromArgb(Color));
        }
    }
}