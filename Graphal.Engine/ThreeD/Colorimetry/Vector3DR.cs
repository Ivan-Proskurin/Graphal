﻿using System;

using Graphal.Engine.ThreeD.Geometry;

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

        public Vector3D ToVector3D()
        {
            return new Vector3D((int)Math.Round(X), (int)Math.Round(Y), (int)Math.Round(Z));
        }
    }
}