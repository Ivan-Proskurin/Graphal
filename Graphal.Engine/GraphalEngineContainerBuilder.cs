using Graphal.Engine.Abstractions.Profile;
using Graphal.Engine.Abstractions.TwoD.Rendering;
using Graphal.Engine.Profile;
using Graphal.Engine.TwoD.Rendering;

using Microsoft.Extensions.DependencyInjection;

namespace Graphal.Engine
{
    public static class GraphalEngineContainerBuilder
    {
        public static IServiceCollection AddDefaultPerformanceProfiler(this IServiceCollection services)
        {
            return services.AddSingleton<IPerformanceProfiler, PerformanceProfiler>();
        }

        public static IServiceCollection AddEngine2D(this IServiceCollection services)
        {
            return services
                .AddTransient<IRenderingMap, RenderingMap>()
                .AddSingleton<ICanvas2D, Canvas2D>()
                .AddSingleton<IScene2D, Scene2D>();
        }
    }
}