using System.Threading.Tasks;

using Graphal.Engine.Abstractions.Logging;
using Graphal.Engine.Abstractions.Profile;
using Graphal.Tools.Abstractions.Windows;
using Graphal.VisualDebug.Abstractions;
using Graphal.VisualDebug.Abstractions.Canvas;
using Graphal.VisualDebug.Abstractions.Logging;

namespace Graphal.VisualDebug.ViewModels
{
    public class MainWindowViewModel : IMainWindowViewModel
    {
        private const string ViewModelName = "graphalMain";

        private readonly WindowAppearance _defaultWindowAppearance = new WindowAppearance
        {
            Width = 640,
            Height = 480,
            Left = 100,
            Top = 100,
        };

        private readonly ILogger _logger;
        private readonly IPerformanceProfiler _performanceProfiler;
        private readonly IWindowAppearanceService _windowAppearanceService;
        private readonly ICanvasViewModel3d _canvasViewModel3d;

        public MainWindowViewModel(
            ILogger logger,
            IPerformanceProfiler performanceProfiler,
            IWindowAppearanceService windowAppearanceService,
            ICanvasViewModel canvasViewModel,
            ICanvasViewModel3d canvasViewModel3d,
            ILogConsoleViewModel logConsoleViewModel)
        {
            _logger = logger;
            _performanceProfiler = performanceProfiler;
            _windowAppearanceService = windowAppearanceService;
            _canvasViewModel3d = canvasViewModel3d;
            Canvas = canvasViewModel;
            Canvas3d = canvasViewModel3d;
            LogConsole = logConsoleViewModel;
        }

        public string WindowTitle { get; set; } = "Graphal Visual Debug Tool";

        public int WindowWidth { get; set; }

        public int WindowHeight { get; set; }

        public int PositionLeft { get; set; }

        public int PositionTop { get; set; }

        public ILogConsoleViewModel LogConsole { get; }

        public ICanvasViewModel Canvas { get; }

        public ICanvasViewModel3d Canvas3d { get; }

        public async Task InitializeAsync()
        {
            using (var session = _performanceProfiler.CreateSession())
            {
                await RestoreAppearanceAsync();
                LogConsole.Initialize();
                session.LogWithPerformance("Application started");
            }
        }

        public async Task CloseAsync()
        {
            await Canvas.StoreSceneAsync();
            await SaveAppearanceAsync();
        }

        private async Task RestoreAppearanceAsync()
        {
            var appearance = await _windowAppearanceService.LoadAsync(ViewModelName) ?? _defaultWindowAppearance;
            WindowWidth = appearance.Width;
            WindowHeight = appearance.Height;
            PositionLeft = appearance.Left;
            PositionTop = appearance.Top;
        }

        private Task SaveAppearanceAsync()
        {
            return _windowAppearanceService.SaveAsync(ViewModelName, new WindowAppearance
            {
                Width = WindowWidth,
                Height = WindowHeight,
                Left = PositionLeft,
                Top = PositionTop,
            });
        }
    }
}