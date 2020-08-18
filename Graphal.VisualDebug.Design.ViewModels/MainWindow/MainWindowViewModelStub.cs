using System.Threading.Tasks;

using Graphal.VisualDebug.Abstractions;
using Graphal.VisualDebug.Abstractions.Canvas;
using Graphal.VisualDebug.Abstractions.Logging;
using Graphal.VisualDebug.Design.ViewModels.Canvas;
using Graphal.VisualDebug.Design.ViewModels.LogConsole;

namespace Graphal.VisualDebug.Design.ViewModels.MainWindow
{
    public class MainWindowViewModelStub : IMainWindowViewModel
    {
        public MainWindowViewModelStub()
        {
            LogConsole = new LogConsoleViewModelStub();
            Canvas = new CanvasViewModelStub();
            Canvas3d = new CanvasViewModel3dStub();
        }

        public string WindowTitle { get; set; } = "Graphal Visual Debug Tool Stub";

        public int WindowWidth { get; set; } = 100;

        public int WindowHeight { get; set; } = 300;

        public int PositionLeft { get; set; }

        public int PositionTop { get; set; }

        public ILogConsoleViewModel LogConsole { get; }

        public ICanvasViewModel Canvas { get; }

        public ICanvasViewModel3d Canvas3d { get; }

        public Task InitializeAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task CloseAsync()
        {
            throw new System.NotImplementedException();
        }
    }
}