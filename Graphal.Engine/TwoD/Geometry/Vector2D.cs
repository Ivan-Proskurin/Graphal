using Graphal.Engine.Persistence.TwoD.Geometry;

namespace Graphal.Engine.TwoD.Geometry
{
    public readonly struct Vector2D
    {
        public Vector2D(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X { get; }

        public int Y { get; }

        public Vector2Ds ToVector2Ds()
        {
            return new Vector2Ds
            {
                X = X,
                Y = Y,
            };
        }

        public static Vector2D operator +(Vector2D vector1, Vector2D vector2)
        {
            return new Vector2D(vector1.X + vector2.X, vector1.Y + vector2.Y);
        }

        public static Vector2D operator -(Vector2D vector1, Vector2D vector2)
        {
            return new Vector2D(vector1.X - vector2.X, vector1.Y - vector2.Y);
        }
    }
}