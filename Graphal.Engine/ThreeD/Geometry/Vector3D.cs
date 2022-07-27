using System;
using System.Linq;

using Graphal.Engine.ThreeD.Colorimetry;

namespace Graphal.Engine.ThreeD.Geometry
{
    public struct Vector3D
    {
        public Vector3D(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }
        
        public int X { get; }

        public int Y { get; }

        public int Z { get; }

        public Vector3DR ToVector3DR()
        {
            return new Vector3DR(X, Y, Z);
        }

        public Vector3D Subtract(Vector3D v)
        {
            return new Vector3D(X - v.X, Y - v.Y, Z - v.Z);
        }

        public Vector3D Add(Vector3D v)
        {
            return new Vector3D(X + v.X, Y + v.Y, Z + v.Z);
        }

        public Vector3D Multiply(double k)
        {
            var x = X * k;
            var y = Y * k;
            var z = Z * k;
            return new Vector3D((int)x, (int)y, (int)z);
        }

        public static double ScalarNormalizedMultiplication(Vector3D v1, Vector3DR v2)
        {
            var len1 = Math.Sqrt(v1.X * v1.X + v1.Y * v1.Y + v1.Z * v1.Z);
            var len2 = Math.Sqrt(v2.X * v2.X + v2.Y * v2.Y + v2.Z * v2.Z);
            return (v1.X * v2.X + v1.Y * v2.Y + v1.Z * v2.Z) / len1 / len2;
        }

        public static Vector3D Average(params Vector3D[] vectors)
        {
            if (!vectors.Any())
            {
                return new Vector3D();
            }

            var sumVector = vectors.Aggregate(new Vector3D(0, 0, 0), (result, current) => result.Add(current));
            var len = vectors.Length;
            return new Vector3D(sumVector.X / len, sumVector.Y / len, sumVector.Z / len);
        }

        public static void Swap(ref Vector3D v1, ref Vector3D v2)
        {
            var v = v1;
            v1 = v2;
            v2 = v;
        }
    }
}