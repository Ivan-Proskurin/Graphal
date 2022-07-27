using Graphal.Engine.Abstractions.IntersectBehaviours;
using Graphal.Engine.Abstractions.Profile;
using Graphal.Engine.Abstractions.ThreeD.Animation;
using Graphal.Engine.Abstractions.ThreeD.Rendering;
using Graphal.Engine.Abstractions.TwoD.Rendering;
using Graphal.Engine.Profile;
using Graphal.Engine.ThreeD.Animation;
using Graphal.Engine.ThreeD.Rendering;
using Graphal.Engine.TwoD.IntersectBehaviours;
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
                .AddSingleton<IIntersectionFactory, IntersectionFactory>()
                .AddSingleton<ICanvas2D, Canvas2D>()
                .AddSingleton<IScene2D, Scene2D>()
                .AddSingleton<IScene3D, Scene3D>()
                .AddSingleton<IAnimationProcessor, AnimationProcessor>();
        }
    }
}