using System.Threading.Tasks;

using Graphal.VisualDebug.Abstractions.Canvas;
using Graphal.VisualDebug.Abstractions.Logging;

namespace Graphal.VisualDebug.Abstractions
{
    public interface IMainWindowViewModel
    {
        string WindowTitle { get; set; }

        int WindowWidth { get; set; }

        int WindowHeight { get; set; }

        int PositionLeft { get; set; }

        int PositionTop { get; set; }

        ILogConsoleViewModel LogConsole { get; }

        ICanvasViewModel Canvas { get; }

        Task InitializeAsync();

        Task CloseAsync();
    }
}