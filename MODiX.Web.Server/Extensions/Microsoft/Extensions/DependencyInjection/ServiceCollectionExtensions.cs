using System;

using Microsoft.Extensions.Options;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddScopedWithAlias<TService, TAlias>(this IServiceCollection services)
                where TService : class, TAlias
                where TAlias : class
            => services
                .AddScoped<TService>()
                .AddScoped<TAlias>(serviceProvider => serviceProvider.GetRequiredService<TService>());

        public static IServiceCollection PostConfigureWith<TOptions, TService>(this IServiceCollection services, Action<TOptions, TService> configureOptions)
                where TOptions : class
                where TService : notnull
            => services.AddTransient<IPostConfigureOptions<TOptions>>(serviceProvider => new OptionsWithPostConfiguration<TOptions, TService>(configureOptions, serviceProvider));

        private class OptionsWithPostConfiguration<TOptions, TService>
                : IPostConfigureOptions<TOptions>
            where TOptions : class
            where TService : notnull
        {
            public OptionsWithPostConfiguration(
                Action<TOptions, TService>  configureOptions,
                IServiceProvider            serviceProvider)
            {
                _configureOptions   = configureOptions;
                _serviceProvider    = serviceProvider;
            }

            public void PostConfigure(string name, TOptions options)
                => _configureOptions.Invoke(options, _serviceProvider.GetRequiredService<TService>());

            private readonly Action<TOptions, TService> _configureOptions;
            private readonly IServiceProvider           _serviceProvider;
        }
    }
}
