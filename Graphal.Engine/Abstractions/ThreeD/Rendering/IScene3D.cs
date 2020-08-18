using System.Threading.Tasks;

using Graphal.Engine.Abstractions.TwoD.Rendering;
using Graphal.Engine.ThreeD.Geometry;
using Graphal.Engine.ThreeD.Primitives;

namespace Graphal.Engine.Abstractions.ThreeD.Rendering
{
    public interface IScene3D
    {
        void Append(Triangle3D triangle);

        Task InitializeAsync();

        Task RenderAsync();

        Task RotateXZAsync(int angles);

        Task RotateYZAsync(int angles);

        Task StopRotationAsync();

        void SetObjectPosition(Vector3D v);

        Task MoveSceneCloser(double grade);

        Task MoveSceneFurther(double grade);

        event FpsChangedEventHandler FpsChanged;
    }
}