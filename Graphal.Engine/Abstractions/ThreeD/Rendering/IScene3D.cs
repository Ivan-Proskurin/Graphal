using System.Collections.Generic;
using System.Threading.Tasks;

using Graphal.Engine.ThreeD.Geometry;
using Graphal.Engine.ThreeD.Primitives;

namespace Graphal.Engine.Abstractions.ThreeD.Rendering
{
    public interface IScene3D
    {
        void Append(Triangle3D triangle);

        void Append(IEnumerable<Triangle3D> triangles);

        void Append(Object3D object3D);

        void Append(Edge3D edge);

        Task InitializeAsync();

        Task RenderAsync();

        void SetObjectPosition(Vector3D v);

        Task MoveSceneCloser(double grade);

        Task MoveSceneFurther(double grade);

        Task StartRotateAsync(double x, double y);

        Task ContinueRotateAsync(double x, double y);

        Task StopRotateAsync(double x, double y);

        Task RotateCubeDimension(bool reverse);

        Task RotateCubeDimension(int cubeDimension, bool reverse);
    }
}