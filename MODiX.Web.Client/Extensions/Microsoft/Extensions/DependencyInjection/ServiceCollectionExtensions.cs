namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSingletonAlias<TService, TImplementation>(this IServiceCollection services)
                where TService : class
                where TImplementation : class, TService
            => services.AddSingleton<TService>(serviceProvider => serviceProvider.GetRequiredService<TImplementation>());
    }
}
