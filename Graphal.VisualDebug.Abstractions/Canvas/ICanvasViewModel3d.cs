using System.Threading.Tasks;

namespace Graphal.VisualDebug.Abstractions.Canvas
{
    public interface ICanvasViewModel3d
    {
        object ImageSource { get; }

        void Resize(int width, int height);

        Task InitializeAsync();

        Task MoveCloser();

        Task MoveFurther();

        Task StartRotateAsync(int x, int y);

        Task ContinueRotateAsync(int x, int y);

        Task StopRotateAsync(int x, int y);
    }
}