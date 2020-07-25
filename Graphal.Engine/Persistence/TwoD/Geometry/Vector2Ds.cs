using Graphal.Engine.TwoD.Geometry;

namespace Graphal.Engine.Persistence.TwoD.Geometry
{
    public class Vector2Ds
    {
        public int X { get; set; }

        public int Y { get; set; }

        public Vector2D ToVector2D()
        {
            return new Vector2D(X, Y);
        }
    }
}