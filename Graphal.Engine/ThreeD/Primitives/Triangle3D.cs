using System;
using System.Data;
using System.Drawing;
using System.Threading;

using Graphal.Engine.ThreeD.Colorimetry;
using Graphal.Engine.ThreeD.Geometry;
using Graphal.Engine.TwoD.Primitives;

namespace Graphal.Engine.ThreeD.Primitives
{
    public class Triangle3D
    {
        private Vector3D _origv1;
        private Vector3D _origv2;
        private Vector3D _origv3;
        private Vector3D _v1;
        private Vector3D _v2;
        private Vector3D _v3;
        private readonly Color _color;
        private int _normalZ;
        private Vector3D _position;
        private double _radiansXZ;
        private double _radiansYZ;
        private double _deepLevel;
        private Func<Vector3D, Vector3D, Vector3D, double> _deepPointFunc;

        public Triangle3D(Vector3D v1, Vector3D v2, Vector3D v3, Color color, Func<Vector3D, Vector3D, Vector3D, double> deepPointFunc = null)
        {
            _origv1 = v1;
            _origv2 = v2;
            _origv3 = v3;
            _v1 = v1;
            _v2 = v2;
            _v3 = v3;
            _color = color;
            _deepPointFunc = deepPointFunc;
        }

        public Triangle2D Project(int d, ColorimetryInfo colorimetry)
        {
            return new Triangle2D(_v1.Project(d), _v2.Project(d), _v3.Project(d), ApplyColorimetry(_color, colorimetry));
        }

        public void SetPosition(Vector3D position)
        {
            _position = position;
            Transform();
        }

        public void MoveCloserByGrade(double grade)
        { 
            var direction = _position.Multiply(-grade);
            Move(direction);
        }

        public void MoveFurtherByGrade(double grade)
        {
            var direction = _position.Multiply(grade);
            Move(direction);
        }
        
        public void Move(Vector3D direction)
        {
            _position = _position.Add(direction);
            _origv1 = _origv1.Add(direction);
            _origv2 = _origv2.Add(direction);
            _origv3 = _origv3.Add(direction);
            Transform();
        }

        public void RotateXZ(double radians)
        {
            _radiansXZ += radians;
            Transform();
        }

        public void RotateYZ(double radians)
        {
            _radiansYZ += radians;
            Transform();
        }

        public void Multiplay(double rate)
        {
            _origv1 = _origv1.Multiply(rate);
            _origv2 = _origv2.Multiply(rate);
            _origv3 = _origv3.Multiply(rate);
        }

        public double DeepLevel => _deepLevel;

        public Vector3D OrientationVector
        {
            get
            {
                var x = (_v1.X + _v2.X + _v3.X) / 3;
                var y = (_v1.Y + _v2.Y + _v3.Y) / 3;
                var z = (_v1.Z + _v2.Z + _v3.Z) / 3;
                return new Vector3D(x, y, z);
            }
        }

        public int NormalZ => _normalZ;
        
        private void Transform()
        {
            _v1 = _origv1.Subtract(_position).ToVector3DR().RotateXZ(_radiansXZ).RotateYZ(_radiansYZ).ToVector3D().Add(_position);
            _v2 = _origv2.Subtract(_position).ToVector3DR().RotateXZ(_radiansXZ).RotateYZ(_radiansYZ).ToVector3D().Add(_position);
            _v3 = _origv3.Subtract(_position).ToVector3DR().RotateXZ(_radiansXZ).RotateYZ(_radiansYZ).ToVector3D().Add(_position);
            _deepLevel = CalculateDeepLevel();
            _normalZ = CalculateNormalZ();
        }

        private Vector3D CalculateFullNormal()
        {
            var v1 = new Vector3D(_v1.X - _v3.X, _v1.Y - _v3.Y, _v1.Z - _v3.Z);
            var v2 = new Vector3D(_v2.X - _v3.X, _v2.Y - _v3.Y, _v2.Z - _v3.Z);
            var x = v1.Y * v2.Z - v1.Z * v2.Y;
            var y = v1.Z * v2.X - v1.X * v2.Z;
            var z = v1.X * v2.Y - v1.Y * v2.X;
            return new Vector3D(x, y, z);
        }

        private int CalculateNormalZ()
        {
            var v1 = new Vector3D(_v1.X - _v3.X, _v1.Y - _v3.Y, _v1.Z - _v3.Z);
            var v2 = new Vector3D(_v2.X - _v3.X, _v2.Y - _v3.Y, _v2.Z - _v3.Z);
            return v1.X * v2.Y - v1.Y * v2.X;
        }

        private double CalculateDeepLevel()
        {
            if (_deepPointFunc != null)
            {
                return _deepPointFunc(_v1, _v2, _v3);
            }

            var x = (_v1.X + _v2.X + _v3.X) / 3;
            var y = (_v1.Y + _v2.Y + _v3.Y) / 3;
            var z = (_v1.Z + _v2.Z + _v3.Z) / 3;
            return Math.Sqrt(x * x + y * y + z * z);
        }

        private Color ApplyColorimetry(Color original, ColorimetryInfo colorimetry)
        {
            var normal = CalculateFullNormal();
            var lightFallNormalized = colorimetry.LightFall.Normalize();
            var g = ApplyColorimetryToComponent(original.G, colorimetry, lightFallNormalized, normal);
            var r = ApplyColorimetryToComponent(original.R, colorimetry, lightFallNormalized, normal);
            var b = ApplyColorimetryToComponent(original.B, colorimetry, lightFallNormalized, normal);
            return Color.FromArgb(r, g, b);
        }

        private static byte ApplyColorimetryToComponent(byte original, ColorimetryInfo colorimetry, Vector3D light, Vector3D normal)
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