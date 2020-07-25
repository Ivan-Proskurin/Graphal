using Graphal.Engine.Abstractions.Logging;
using Graphal.Tools.Abstractions.Application;
using Graphal.Tools.Abstractions.Serialization;
using Graphal.Tools.Abstractions.Windows;
using Graphal.Tools.Services.Application;
using Graphal.Tools.Services.Logging;
using Graphal.Tools.Services.Serialization;
using Graphal.Tools.Services.Windows;
using Microsoft.Extensions.DependencyInjection;

namespace Graphal.Tools.Services
{
    public static class ServicesContainerBuilder
    {
        public static IServiceCollection AddGraphalToolsServices(this IServiceCollection services)
        {
            return services
                .AddSingleton<ILogger, Logger>()
                .AddSingleton(provider => (ILogObserver)provider.GetRequiredService<ILogger>())
                .AddSingleton<IJsonSerializationService, JsonSerializationService>()
                .AddSingleton<IApplicationStandardPaths, ApplicationStandardPaths>()
                .AddSingleton<IWindowAppearanceService, WindowAppearanceService>();
        }
    }
}