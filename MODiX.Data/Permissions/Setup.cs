using Microsoft.Extensions.DependencyInjection;

namespace Modix.Data.Permissions
{
    public static class Setup
    {
        public static IServiceCollection AddPermissions(this IServiceCollection services)
            => services.AddScoped<IPermissionsRepository, PermissionsRepository>();
    }
}
