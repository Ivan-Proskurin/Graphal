using System.Threading.Tasks;

using Graphal.VisualDebug.Abstractions.Canvas;

namespace Graphal.VisualDebug.Design.ViewModels.Canvas
{
    public class CanvasViewModelStub : ICanvasViewModel
    {
        public object ImageSource => null;

        public Task InitializeAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task StoreSceneAsync()
        {
            throw new System.NotImplementedException();
        }

        public void SetPoint(int x, int y)
        {
            throw new System.NotImplementedException();
        }

        public void Resize(int width, int height)
        {
            throw new System.NotImplementedException();
        }

        public void BeginShift(int x, int y)
        {
            throw new System.NotImplementedException();
        }

        public Task BeginShiftAsync(int x, int y)
        {
            throw new System.NotImplementedException();
        }

        public void Shift(int x, int y)
        {
            throw new System.NotImplementedException();
        }

        public Task ShiftAsync(int x, int y)
        {
            throw new System.NotImplementedException();
        }

        public void EndShift(int x, int y)
        {
            throw new System.NotImplementedException();
        }

        public Task EndShiftAsync(int x, int y)
        {
            throw new System.NotImplementedException();
        }

        public void LogMessage(string message)
        {
            throw new System.NotImplementedException();
        }
    }
}