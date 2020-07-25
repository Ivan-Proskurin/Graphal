using Graphal.Engine;
using Graphal.Engine.Abstractions.TwoD.Rendering;
using Graphal.Tools.Abstractions.Application;
using Graphal.Tools.Services;
using Graphal.Tools.Storage;
using Graphal.VisualDebug.Abstractions.Canvas;
using Graphal.VisualDebug.Rendering;
using Graphal.VisualDebug.ViewModels;

using Microsoft.Extensions.DependencyInjection;

namespace Graphal.VisualDebug
{
    public static class VisualDebugContainerBuilder
    {
        public static IServiceCollection BuildVisualDebugContainer(this IServiceCollection services)
        {
            return services
                .AddSingleton<IApplicationInfo, ApplicationInfo>()
                .AddSingleton<IRenderingSettingsProvider, RenderingSettingsProvider>()
                .AddSingleton<ICanvas, BitmapCanvas>()
                .AddSingleton(provider => provider.GetRequiredService<ICanvas>() as IBitmapCanvas)
                .AddEngine2D()
                .AddDefaultPerformanceProfiler()
                .AddFileStorage()
                .AddMemoryLogStorage()
                .AddGraphalToolsServices()
                .AddVisualDebugViewModels();
        }
    }
}