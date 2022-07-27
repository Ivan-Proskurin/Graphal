using System.Threading.Tasks;

namespace Graphal.VisualDebug.Abstractions.Canvas
{
    public interface ICanvasViewModel3d
    {
        object ImageSource { get; }

        void Resize(int width, int height);

        Task InitializeAsync();

        Task MoveCloser(double grade);

        Task MoveFurther(double grade);

        Task StartRotateAsync(double x, double y);

        Task ContinueRotateAsync(double x, double y);

        Task StopRotateAsync(double x, double y);

        Task RotateCubeDimension(bool reverse);
    }
}