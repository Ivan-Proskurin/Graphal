using Graphal.Tools.Storage.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Graphal.Tools.Storage
{
    public static class StorageContainerBuilder
    {
        public static IServiceCollection AddFileStorage(this IServiceCollection services)
        {
            return services.AddSingleton<IFileStorage, FileStorage>();
        }

        public static IServiceCollection AddMemoryLogStorage(this IServiceCollection services)
        {
            return services.AddSingleton<ILogStorage, MemoryLogStorage>();
        }
    }
}