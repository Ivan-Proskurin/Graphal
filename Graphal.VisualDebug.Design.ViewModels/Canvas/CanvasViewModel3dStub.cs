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

        public Task RotateLeftAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task RotateRightAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task RotateUpAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task RotateDownAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task StopRotationAsync()
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
    }
}