using Graphal.Engine.ThreeD.Colorimetry;

namespace Graphal.RubiksCube.Core
{
    public class PlaneRotationInfo
    {
        private Vector3DR _rotateVector;
        private Vector3DR _rotatePosition;
        
        public Vector3DR Vector { get; set; }

        public Vector3DR Position { get; set; }

        public void StartRotation()
        {
            _rotateVector = Vector;
            _rotatePosition = Position;
        }

        public void Rotate(Vector3DR basePosition, double radiansXZ, double radiansYZ)
        {
            Vector = _rotateVector.RotateXZ(radiansXZ).RotateYZ(radiansYZ);
            Position = _rotatePosition.Subtract(basePosition).RotateXZ(radiansXZ).RotateYZ(radiansYZ).Add(basePosition);
        }

        public void StopRotation()
        {
            _rotateVector = null;
            _rotatePosition = null;
        }
    }
}