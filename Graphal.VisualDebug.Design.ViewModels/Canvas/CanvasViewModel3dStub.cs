using System.Threading.Tasks;

using Graphal.VisualDebug.Abstractions.Canvas;

namespace Graphal.VisualDebug.Design.ViewModels.Canvas
{
    public class CanvasViewModel3dStub : ICanvasViewModel3d
    {
        public object ImageSource { get; }
        public void Resize(int width, int height)
        {
            throw new System.NotImplementedException();
        }

        public Task InitializeAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task MoveCloser()
        {
            throw new System.NotImplementedException();
        }

        public Task MoveFurther()
        {
            throw new System.NotImplementedException();
        }

        public Task StartRotateAsync(int x, int y)
        {
            throw new System.NotImplementedException();
        }

        public Task ContinueRotateAsync(int x, int y)
        {
            throw new System.NotImplementedException();
        }

        public Task StopRotateAsync(int x, int y)
        {
            throw new System.NotImplementedException();
        }

        public void DetectTriangles(int x, int y)
        {
            throw new System.NotImplementedException();
        }
    }
}