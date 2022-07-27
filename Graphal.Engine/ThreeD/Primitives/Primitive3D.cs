using Graphal.Engine.ThreeD.Colorimetry;
using Graphal.Engine.ThreeD.Geometry;
using Graphal.Engine.TwoD.Primitives;

namespace Graphal.Engine.ThreeD.Primitives
{
    public abstract class Primitive3D
    {
        public abstract void SetPosition(Vector3D position);

        public abstract void MoveCloserByGrade(double grade);

        public abstract void MoveFurtherByGrade(double grade);

        public abstract int CalculateNormalZ();

        public abstract double DeepLevel();

        public abstract Primitive2D Project(int d, ColorimetryInfo colorimetry, object owner = null);

        public abstract void StartRotation();

        public abstract void Rotate(double radiansXZ, double radiansYZ);

        public abstract void StopRotation();
    }
}