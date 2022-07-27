namespace Graphal.Engine.TwoD.Geometry
{
    public class Vector2DR
    {
        public Vector2DR(double x, double y)
        {
            X = x;
            Y = y;
        }

        public double X { get; }

        public double Y { get; }

        public Vector2DR Subtract(Vector2DR other)
        {
            return new Vector2DR(X - other.X, Y - other.Y);
        }
    }
}