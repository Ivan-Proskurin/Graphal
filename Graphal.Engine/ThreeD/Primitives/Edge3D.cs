using System.Drawing;

using Graphal.Engine.ThreeD.Colorimetry;
using Graphal.Engine.ThreeD.Geometry;
using Graphal.Engine.TwoD.Primitives;

namespace Graphal.Engine.ThreeD.Primitives
{
    public class Edge3D : Primitive3D
    {
        private Vector3D _originalV1;
        private Vector3D _originalV2;
        private Vector3DR _v1;
        private Vector3DR _v2;
        private readonly Color _color;
        private Vector3D _position;
        private double _baseRadiansXZ;
        private double _baseRadiansYZ;
        private double _radiansXZ;
        private double _radiansYZ;

        public Edge3D(Vector3D originalV1, Vector3D originalV2, Color color)
        {
            _originalV1 = originalV1;
            _originalV2 = originalV2;
            _v1 = _originalV1.ToVector3DR();
            _v2 = _originalV2.ToVector3DR();
            _color = color;
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

        public override int NormalZ { get; } = 1;

        public override double DeepLevel()
        {
            return 0;
        }

        public override Primitive2D Project(int d, ColorimetryInfo colorimetry)
        {
            var edge2d = new Edge2D(_v1.Project(d), _v2.Project(d), _color);
            return edge2d;
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

        private void Transform()
        {
            _v1 = _originalV1.Subtract(_position).ToVector3DR().RotateXZ(_radiansXZ).RotateYZ(_radiansYZ).Add(_position);
            _v2 = _originalV2.Subtract(_position).ToVector3DR().RotateXZ(_radiansXZ).RotateYZ(_radiansYZ).Add(_position);
        }
        
        public void Move(Vector3D direction)
        {
            _position = _position.Add(direction);
            _originalV1 = _originalV1.Add(direction);
            _originalV2 = _originalV2.Add(direction);
            Transform();
        }
    }
}