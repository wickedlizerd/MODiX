using Microsoft.Extensions.DependencyInjection;

using Modix.Business.Messaging;

namespace Modix.Business.Guilds.Tracking
{
    public static class Setup
    {
        public static IServiceCollection AddGuildTracking(this IServiceCollection services)
            => services
                .AddSingleton<IGuildTrackingCache, GuildTrackingCache>()
                .AddNotificationHandler<GuildTrackingNotificationHandler>();
    }
}
