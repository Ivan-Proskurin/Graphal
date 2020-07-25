namespace Graphal.VisualDebug.Abstractions.Canvas
{
    public interface ICanvasViewModel
    {
        object ImageSource { get; }

        void Initialize();

        void SetPoint(int x, int y);

        void Resize(int width, int height);

        void BeginShift(int x, int y);

        void Shift(int x, int y);

        void EndShift(int x, int y);
    }
}