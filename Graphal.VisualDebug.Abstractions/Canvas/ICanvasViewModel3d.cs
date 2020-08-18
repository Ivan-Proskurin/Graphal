using System.Threading.Tasks;

namespace Graphal.VisualDebug.Abstractions.Canvas
{
    public interface ICanvasViewModel3d
    {
        object ImageSource { get; }

        void Resize(int width, int height);

        Task InitializeAsync();

        Task RotateLeftAsync();

        Task RotateRightAsync();

        Task RotateUpAsync();

        Task RotateDownAsync();

        Task StopRotationAsync();

        Task MoveCloser();

        Task MoveFurther();
    }
}