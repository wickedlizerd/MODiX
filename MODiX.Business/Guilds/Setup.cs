using Microsoft.Extensions.DependencyInjection;

using Modix.Business.Guilds.Tracking;

namespace Modix.Business.Guilds
{
    public static class Setup
    {
        public static IServiceCollection AddGuilds(this IServiceCollection services)
            => services.AddGuildTracking();
    }
}
