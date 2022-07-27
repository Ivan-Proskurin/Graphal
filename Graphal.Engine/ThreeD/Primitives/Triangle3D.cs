using System;
using System.Drawing;

using Graphal.Engine.ThreeD.Colorimetry;
using Graphal.Engine.ThreeD.Geometry;
using Graphal.Engine.TwoD.Primitives;

namespace Graphal.Engine.ThreeD.Primitives
{
    public class Triangle3D : Primitive3D
    {
        private Vector3DR _v1;
        private Vector3DR _v2;
        private Vector3DR _v3;
        private readonly Color _color;
        private Vector3D _position;
        private Vector3DR _rotateV1;
        private Vector3DR _rotateV2;
        private Vector3DR _rotateV3;

        public Triangle3D(Vector3D v1, Vector3D v2, Vector3D v3, Color color, object owner = null)
        {
            _v1 = v1.ToVector3DR();
            _v2 = v2.ToVector3DR();
            _v3 = v3.ToVector3DR();
            _color = color;
        }

        public override Primitive2D Project(int d, ColorimetryInfo colorimetry, object owner = null)
        {
            return new Triangle2D(_v1.Project(d), _v2.Project(d), _v3.Project(d), ApplyColorimetry(_color, colorimetry), owner);
        }

        public override void StartRotation()
        {
            _rotateV1 = _v1;
            _rotateV2 = _v2;
            _rotateV3 = _v3;
        }

        public override void SetPosition(Vector3D position)
        {
            _position = position;
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
            _rotateV1 = _rotateV1?.Add(direction);
            _rotateV2 = _rotateV2?.Add(direction);
            _rotateV3 = _rotateV3?.Add(direction);
            _v1 = _v1.Add(direction);
            _v2 = _v2.Add(direction);
            _v3 = _v3.Add(direction);
        }

        public void RotateAroundVector(Vector3DR position, Vector3DR vector, double radians)
        {
            _v1 = RotatePointAroundVector(_v1, position, vector, radians);
            _v2 = RotatePointAroundVector(_v2, position, vector, radians);
            _v3 = RotatePointAroundVector(_v3, position, vector, radians);
         }

        private static Vector3DR RotatePointAroundVector(Vector3DR v, Vector3DR position, Vector3DR vector, double radians)
        {
            return v.Subtract(position).RotateAroundVector(vector, radians).Add(position);
        }

        public override double DeepLevel()
        {
            var x = (_v1.X + _v2.X + _v3.X) / 3;
            var y = (_v1.Y + _v2.Y + _v3.Y) / 3;
            var z = (_v1.Z + _v2.Z + _v3.Z) / 3;
            return Math.Sqrt(x * x + y * y + z * z);
        }

        public override void Rotate(double radiansXZ, double radiansYZ)
        {
            _v1 = _rotateV1.Subtract(_position).RotateXZ(radiansXZ).RotateYZ(radiansYZ).Add(_position);
            _v2 = _rotateV2.Subtract(_position).RotateXZ(radiansXZ).RotateYZ(radiansYZ).Add(_position);
            _v3 = _rotateV3.Subtract(_position).RotateXZ(radiansXZ).RotateYZ(radiansYZ).Add(_position);
        }

        public override void StopRotation()
        {
            _rotateV1 = null;
            _rotateV2 = null;
            _rotateV3 = null;
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

        public override int CalculateNormalZ()
        {
            var normal = CalculateFullNormal();
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