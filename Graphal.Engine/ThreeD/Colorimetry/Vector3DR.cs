using System;

using Graphal.Engine.ThreeD.Geometry;
using Graphal.Engine.TwoD.Geometry;

namespace Graphal.Engine.ThreeD.Colorimetry
{
    public class Vector3DR
    {
        public Vector3DR(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public double X { get; }
        
        public double Y { get; }
        
        public double Z { get; }
        
        public Vector3DR RotateXZ(double radians)
        {
            var sin = Math.Sin(radians);
            var cos = Math.Cos(radians);
            var z = Z * cos + X * sin;
            var x = -Z * sin + X * cos;
            return new Vector3DR(x, Y, z);
        }
        
        public Vector3DR RotateYZ(double radians)
        {
            var sin = Math.Sin(radians);
            var cos = Math.Cos(radians);
            var z = Z * cos + Y * sin;
            var y = -Z * sin + Y * cos;
            return new Vector3DR(X, y, z);
        }

        public Vector3DR Add(Vector3D other)
        {
            return new Vector3DR(X + other.X, Y + other.Y, Z + other.Z);
        }

        public Vector3DR Add(Vector3DR other)
        {
            return new Vector3DR(X + other.X, Y + other.Y, Z + other.Z);
        }

        public Vector3DR Subtract(Vector3DR other)
        {
            return new Vector3DR(X - other.X, Y - other.Y, Z - other.Z);
        }

        public Vector3DR Subtract(Vector3D other)
        {
            return new Vector3DR(X - other.X, Y - other.Y, Z - other.Z);
        }

        public Vector3DR Multiply(double k)
        {
            return new Vector3DR(X * k, Y * k, Z * k);
        }

        public Vector2D Project(int d)
        {
            var x = X * d / Z;
            var y = Y * d / Z;
            return new Vector2D((int)Math.Round(x), (int)Math.Round(y));
        }

        public Vector3DR RotateAroundVector(Vector3DR v, double radians)
        {
            var d = Math.Sqrt(v.X * v.X + v.Z * v.Z);
            if (d == 0)
            {
                return RotateAroundOy(radians);
            }
            return RotateAroundOy(v, d, true)
                .RotateAroundOz(v, d, true)
                .RotateAroundOx(radians)
                .RotateAroundOz(v, d, false)
                .RotateAroundOy(v, d, false);
        }

        private Vector3DR RotateAroundOy(Vector3DR v, double d, bool reverse)
        {
            var sin = v.Z / d;
            if (reverse)
            {
                sin = -sin;
            }
            var cos = v.X / d;
            var x = X * cos - Z * sin;
            var y = Y;
            var z = X * sin + Z * cos;
            return new Vector3DR(x, y, z);
        }

        private Vector3DR RotateAroundOy(double radians)
        {
            var sin = Math.Sin(radians);
            var cos = Math.Cos(radians);
            var x = X * cos - Z * sin;
            var y = Y;
            var z = X * sin + Z * cos;
            return new Vector3DR(x, y, z);
        }

        private Vector3DR RotateAroundOz(Vector3DR v, double d, bool reverse)
        {
            var sin = v.Y;
            if (reverse)
            {
                sin = -sin;
            }
            var cos = d;
            var x = X * cos - Y * sin;
            var y = X * sin + Y * cos;
            var z = Z;
            return new Vector3DR(x, y, z);
        }

        private Vector3DR RotateAroundOx(double radians)
        {
            var sin = Math.Sin(radians);
            var cos = Math.Cos(radians);
            var x = X;
            var y = Y * cos - Z * sin;
            var z = Y * sin + Z * cos;
            return new Vector3DR(x, y, z);
        }
    }
}