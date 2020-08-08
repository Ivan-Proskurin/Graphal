using System.Windows;

using Graphal.Engine;
using Graphal.Engine.Abstractions.TwoD.Rendering;
using Graphal.Tools.Abstractions.Application;
using Graphal.Tools.Services;
using Graphal.Tools.Storage;
using Graphal.VisualDebug.Abstractions.Canvas;
using Graphal.VisualDebug.Abstractions.Wrappers;
using Graphal.VisualDebug.Rendering;
using Graphal.VisualDebug.ViewModels;
using Graphal.VisualDebug.Wrappers;

using Microsoft.Extensions.DependencyInjection;

namespace Graphal.VisualDebug
{
    public static class VisualDebugContainerBuilder
    {
        public static IServiceCollection BuildVisualDebugContainer(this IServiceCollection services)
        {
            return services
                .AddSingleton<IDispatcherWrapper>(new DispatcherWrapper(Application.Current.Dispatcher))
                .AddSingleton<IApplicationInfo, ApplicationInfo>()
                .AddSingleton<IRenderingSettingsProvider, RenderingSettingsProvider>()
                .AddSingleton<IOutputDevice, BitmapOutputDevice>()
                .AddSingleton(provider => provider.GetRequiredService<IOutputDevice>() as IBitmapSource)
                .AddEngine2D()
                .AddDefaultPerformanceProfiler()
                .AddFileStorage()
                .AddMemoryLogStorage()
                .AddGraphalToolsServices()
                .AddVisualDebugViewModels();
        }
    }
}