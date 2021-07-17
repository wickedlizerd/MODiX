using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace Modix.Web.Server.Authorization
{
    public static class Setup
    {
        public static IServiceCollection AddModixAuthorization(this IServiceCollection services)
            => services
                .AddAuthorization()
                .AddSingleton<IAuthorizationPolicyProvider, AuthorizationPolicyProvider>()
                .AddScoped<IAuthorizationHandler, PermissionRequirementHandler>();
    }
}
