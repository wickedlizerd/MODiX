using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Modix.Business.Users.Tracking;

namespace Modix.Business.Users
{
    public static class Setup
    {
        public static IServiceCollection AddUsers(this IServiceCollection services, IConfiguration configuration)
            => services.AddUserTracking(configuration);
    }
}
