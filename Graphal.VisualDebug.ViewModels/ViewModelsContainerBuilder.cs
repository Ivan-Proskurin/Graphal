using Graphal.VisualDebug.Abstractions;
using Graphal.VisualDebug.Abstractions.Canvas;
using Graphal.VisualDebug.Abstractions.Logging;
using Graphal.VisualDebug.ViewModels.Canvas;
using Graphal.VisualDebug.ViewModels.Logging;

using Microsoft.Extensions.DependencyInjection;

namespace Graphal.VisualDebug.ViewModels
{
    public static class VisualDebugViewModelsContainerBuilder
    {
        public static IServiceCollection AddVisualDebugViewModels(this IServiceCollection services)
        {
            return services
                .AddTransient<ILogConsoleViewModel, LogConsoleViewModel>()
                .AddTransient<ICanvasViewModel, CanvasViewModel>()
                .AddTransient<ICanvasViewModel3d, CanvasViewModel3d>()
                .AddTransient<IMainWindowViewModel, MainWindowViewModel>();
        }
    }
}