using Graphal.Engine.Persistence.TwoD.Geometry;
using Graphal.Engine.TwoD.Primitives;

namespace Graphal.Engine.Persistence.TwoD.Primitives
{
    public class Triangle2Ds : Primitive2Ds
    {
        public Vector2Ds V1 { get; set; }

        public Vector2Ds V2 { get; set; }

        public Vector2Ds V3 { get; set; }

        public int Color { get; set; }

        public override Primitive2D ToPrimitive2D()
        {
            return new Triangle2D(V1.ToVector2D(), V2.ToVector2D(), V3.ToVector2D(), System.Drawing.Color.FromArgb(Color));
        }
    }
}