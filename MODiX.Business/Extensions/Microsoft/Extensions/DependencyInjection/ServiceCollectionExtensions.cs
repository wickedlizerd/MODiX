using System;

using Microsoft.Extensions.Hosting;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection Add(this IServiceCollection services, Action<IServiceCollection> onAdding)
        {
            onAdding.Invoke(services);

            return services;
        }

        public static IServiceCollection AddStartupAction<T>(this IServiceCollection services)
                where T : StartupActionBase
            => services.AddTransient<IHostedService, T>();
    }
}
