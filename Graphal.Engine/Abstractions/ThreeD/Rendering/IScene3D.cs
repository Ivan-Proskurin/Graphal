using System.Collections.Generic;
using System.Threading.Tasks;

using Graphal.Engine.Abstractions.TwoD.Rendering;
using Graphal.Engine.ThreeD.Geometry;
using Graphal.Engine.ThreeD.Primitives;

namespace Graphal.Engine.Abstractions.ThreeD.Rendering
{
    public interface IScene3D
    {
        void Append(Triangle3D triangle);

        void Append(IEnumerable<Triangle3D> triangles);

        void Append(Edge3D edge);

        Task InitializeAsync();

        Task RenderAsync();

        void SetObjectPosition(Vector3D v);

        Task MoveSceneCloser(double grade);

        Task MoveSceneFurther(double grade);

        Task StartRotateAsync(int x, int y);

        Task ContinueRotateAsync(int x, int y);

        Task StopRotateAsync(int x, int y);

        event FpsChangedEventHandler FpsChanged;
    }
}