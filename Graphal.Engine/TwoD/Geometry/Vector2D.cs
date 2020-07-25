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

        public static void SortByY(ref Vector2D v1, ref Vector2D v2, ref Vector2D v3)
        {
            if (v1.Y > v2.Y)
            {
                Swap(ref v1, ref v2);
            }

            if (v1.Y > v3.Y)
            {
                Swap(ref v1, ref v3);
            }

            if (v2.Y > v3.Y)
            {
                Swap(ref v2, ref v3);
            }
        }

        public static Vector2D operator +(Vector2D vector1, Vector2D vector2)
        {
            return new Vector2D(vector1.X + vector2.X, vector1.Y + vector2.Y);
        }

        public static Vector2D operator -(Vector2D vector1, Vector2D vector2)
        {
            return new Vector2D(vector1.X - vector2.X, vector1.Y - vector2.Y);
        }

        private static void Swap(ref Vector2D v1, ref Vector2D v2)
        {
            var v = v1;
            v1 = v2;
            v2 = v;
        }
    }
}