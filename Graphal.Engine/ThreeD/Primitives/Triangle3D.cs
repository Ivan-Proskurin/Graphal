using System;
using System.Drawing;

using Graphal.Engine.ThreeD.Colorimetry;
using Graphal.Engine.ThreeD.Geometry;
using Graphal.Engine.TwoD.Primitives;

namespace Graphal.Engine.ThreeD.Primitives
{
    public class Triangle3D : Primitive3D
    {
        private Vector3D _origv1;
        private Vector3D _origv2;
        private Vector3D _origv3;
        private Vector3DR _v1;
        private Vector3DR _v2;
        private Vector3DR _v3;
        private readonly Color _color;
        private int _normalZ;
        private Vector3D _position;
        private double _baseRadiansXZ;
        private double _baseRadiansYZ;
        private double _radiansXZ;
        private double _radiansYZ;

        public Triangle3D(Vector3D v1, Vector3D v2, Vector3D v3, Color color)
        {
            _origv1 = v1;
            _origv2 = v2;
            _origv3 = v3;
            _v1 = v1.ToVector3DR();
            _v2 = v2.ToVector3DR();
            _v3 = v3.ToVector3DR();
            _color = color;
        }

        public override Primitive2D Project(int d, ColorimetryInfo colorimetry)
        {
            return new Triangle2D(_v1.Project(d), _v2.Project(d), _v3.Project(d), ApplyColorimetry(_color, colorimetry));
        }

        public override void SetPosition(Vector3D position)
        {
            _position = position;
            Transform();
        }

        public override void MoveCloserByGrade(double grade)
        { 
            var direction = _position.Multiply(-grade);
            Move(direction);
        }

        public override void MoveFurtherByGrade(double grade)
        {
            var direction = _position.Multiply(grade);
            Move(direction);
        }
        
        private void Move(Vector3D direction)
        {
            _position = _position.Add(direction);
            _origv1 = _origv1.Add(direction);
            _origv2 = _origv2.Add(direction);
            _origv3 = _origv3.Add(direction);
            Transform();
        }

        public override void RotateXZ(double radians)
        {
            _radiansXZ = _baseRadiansXZ + radians;
            Transform();
        }

        public override void ApplyRotationXZ(double radians)
        {
            _baseRadiansXZ += radians;
            _radiansXZ = _baseRadiansXZ;
            Transform();
        }

        public override void RotateYZ(double radians)
        {
            _radiansYZ = _baseRadiansYZ + radians;
            Transform();
        }

        public override void ApplyRotationYZ(double radians)
        {
            _baseRadiansYZ += radians;
            _radiansYZ = _baseRadiansYZ;
            Transform();
        }

        public override double DeepLevel()
        {
            var x = (_v1.X + _v2.X + _v3.X) / 3;
            var y = (_v1.Y + _v2.Y + _v3.Y) / 3;
            var z = (_v1.Z + _v2.Z + _v3.Z) / 3;
            return Math.Sqrt(x * x + y * y + z * z);
        }

        public override int NormalZ => _normalZ;
        
        private void Transform()
        {
            _v1 = _origv1.Subtract(_position).ToVector3DR().RotateXZ(_radiansXZ).RotateYZ(_radiansYZ).Add(_position);
            _v2 = _origv2.Subtract(_position).ToVector3DR().RotateXZ(_radiansXZ).RotateYZ(_radiansYZ).Add(_position);
            _v3 = _origv3.Subtract(_position).ToVector3DR().RotateXZ(_radiansXZ).RotateYZ(_radiansYZ).Add(_position);
            _normalZ = CalculateNormalZ();
        }

        private Vector3DR CalculateFullNormal()
        {
            var v1 = new Vector3DR(_v3.X - _v1.X, _v3.Y - _v1.Y, _v3.Z - _v1.Z);
            var v2 = new Vector3DR(_v3.X - _v2.X, _v3.Y - _v2.Y, _v3.Z - _v2.Z);
            var x = v1.Y * v2.Z - v1.Z * v2.Y;
            var y = v1.Z * v2.X - v1.X * v2.Z;
            var z = v1.X * v2.Y - v1.Y * v2.X;
            return new Vector3DR(x, y, z);
        }

        private int CalculateNormalZ()
        {
            // var v1 = new Vector3DR(_v1.X - _v3.X, _v1.Y - _v3.Y, _v1.Z - _v3.Z);
            // var v2 = new Vector3DR(_v2.X - _v3.X, _v2.Y - _v3.Y, _v2.Z - _v3.Z);
            // var z = v1.X * v2.Y - v1.Y * v2.X;
            // return Math.Sign(z);
            var normal = CalculateFullNormal();
            // var scalar = _visionVector.X * normal.X + _visionVector.Y * normal.Y + _visionVector.Z * normal.Z;
            var x = (_v1.X + _v2.X + _v3.X) / 3;
            var y = (_v1.Y + _v2.Y + _v3.Y) / 3;
            var z = (_v1.Z + _v2.Z + _v3.Z) / 3;
            var scalar = x * normal.X + y * normal.Y + z * normal.Z; 
            return Math.Sign(scalar);
        }

        private Color ApplyColorimetry(Color original, ColorimetryInfo colorimetry)
        {
            var normal = CalculateFullNormal();
            var g = ApplyColorimetryToComponent(original.G, colorimetry, normal);
            var r = ApplyColorimetryToComponent(original.R, colorimetry, normal);
            var b = ApplyColorimetryToComponent(original.B, colorimetry, normal);
            return Color.FromArgb(r, g, b);
        }

        private static byte ApplyColorimetryToComponent(byte original, ColorimetryInfo colorimetry, Vector3DR normal)
        {
            if (original == 0)
            {
                return 0;
            }

            var diffusion = (byte)(original * colorimetry.DiffusionRate);
            var lambertRate = Vector3D.ScalarNormalizedMultiplication(colorimetry.LightFall, normal);
            if (lambertRate >= -1 && lambertRate <= 1)
            {
                lambertRate = (lambertRate + 1) / 2;
                var lambertDiffusion = (byte)((255 - diffusion) * lambertRate * 0.5);
                return (byte)(diffusion + lambertDiffusion);
            }

            return diffusion;
        }
    }
}