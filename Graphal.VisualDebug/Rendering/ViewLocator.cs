using Graphal.VisualDebug.Canvas;

namespace Graphal.VisualDebug.Rendering
{
    public class ViewLocator : IViewLocator
    {
        private readonly App _app;

        public ViewLocator(App app)
        {
            _app = app;
        }

        public CanvasView CanvasView => (_app.MainWindow as MainWindow)?.CanvasView;
    }
}