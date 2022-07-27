using System.Collections.Generic;

namespace Graphal.Engine.ThreeD.Primitives
{
    public abstract class Object3D
    {
        public abstract void MoveCloserByGrade(double grade);

        public abstract void MoveFurtherByGrade(double grade);

        public abstract void StartRotation();

        public abstract void Rotate(double radiansXZ, double radiansYZ);

        public abstract void StopRotation();

        public abstract IEnumerable<Triangle3D> GetTriangles();

        public abstract void RotateCubeDimension(bool reverse);

        public abstract void RotateCubeDimension(int cubeDimension, bool reverse);
    }
}