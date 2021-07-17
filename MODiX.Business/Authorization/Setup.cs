using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Modix.Business.Authorization
{
    public static class Setup
    {
        public static IServiceCollection AddAuthorization(this IServiceCollection services, IConfiguration configuration)
            => services
                .Add(services => services.AddOptions<AuthorizationConfiguration>()
                    .Bind(configuration.GetSection("MODiX:Business:Authorization"))
                    .ValidateDataAnnotations()
                    .ValidateOnStartup())
                .AddSingleton<IAuthorizationPermissionsCache, AuthorizationPermissionsCache>()
                .AddScoped<IAuthorizationService, AuthorizationService>();
    }
}
