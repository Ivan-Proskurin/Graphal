using System.Threading.Tasks;

namespace Graphal.VisualDebug.Abstractions.Canvas
{
    public interface ICanvasViewModel
    {
        object ImageSource { get; }

        Task InitializeAsync();

        Task StoreSceneAsync();

        void SetPoint(int x, int y);

        void Resize(int width, int height);

        Task BeginShiftAsync(int x, int y);

        Task ShiftAsync(int x, int y);

        Task EndShiftAsync(int x, int y);
    }
}