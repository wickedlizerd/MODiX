using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Extensions.Hosting
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddStartupAction<T>(this IServiceCollection services)
                where T : StartupActionBase
            => services.AddTransient<IHostedService, T>();
    }
}
