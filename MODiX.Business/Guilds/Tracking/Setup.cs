using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Modix.Business.Guilds.Tracking
{
    public static class Setup
    {
        public static IServiceCollection AddGuildTracking(this IServiceCollection services)
            => services
                .AddSingleton<IGuildTrackingCache, GuildTrackingCache>()
                .AddReactiveBehavior<GuildTrackingEventListeningBehavior>();
    }
}
