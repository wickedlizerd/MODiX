namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSingletonWithAliases<TService, TAlias1, TAlias2>(this IServiceCollection services)
                where TService : class, TAlias1, TAlias2
                where TAlias1 : class
                where TAlias2 : class
            => services
                .AddSingleton<TService>()
                .AddSingleton<TAlias1>(serviceProvider => serviceProvider.GetRequiredService<TService>())
                .AddSingleton<TAlias2>(serviceProvider => serviceProvider.GetRequiredService<TService>());
    }
}
