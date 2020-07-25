using Graphal.Engine.Abstractions.TwoD.Rendering;

namespace Graphal.VisualDebug.Abstractions.Canvas
{
    public interface IBitmapCanvas : ICanvas
    {
        object Bitmap { get; }

        void SetSize(int width, int height);
    }
}