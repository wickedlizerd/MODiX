using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Extensions.Hosting
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddReactiveBehavior<T>(this IServiceCollection services)
                where T : ReactiveBehaviorBase
            => services.AddSingleton<IHostedService, T>();
    }
}
