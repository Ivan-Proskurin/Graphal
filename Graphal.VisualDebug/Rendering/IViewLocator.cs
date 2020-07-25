using Graphal.VisualDebug.Canvas;

namespace Graphal.VisualDebug.Rendering
{
    public interface IViewLocator
    {
        CanvasView CanvasView { get; }
    }
}