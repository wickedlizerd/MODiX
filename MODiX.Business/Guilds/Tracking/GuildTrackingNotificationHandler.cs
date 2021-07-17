using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using Remora.Discord.API.Abstractions.Gateway.Events;

using Modix.Business.Messaging;

namespace Modix.Business.Guilds.Tracking
{
    public class GuildTrackingNotificationHandler
        : INotificationHandler<IGuildCreate>,
            INotificationHandler<IGuildUpdate>,
            INotificationHandler<IGuildDelete>
    {
        public GuildTrackingNotificationHandler(
            IGuildTrackingCache                         guildTrackingCache,
            ILogger<GuildTrackingNotificationHandler>   logger)
        {
            _guildTrackingCache = guildTrackingCache;
            _logger             = logger;
        }

        public Task HandleNotificationAsync(
                IGuildCreate        notification,
                CancellationToken   cancellationToken)
            => SetEntryAsync(
                new(
                    id:     notification.ID,
                    name:   notification.Name,
                    icon:   notification.Icon),
                cancellationToken);

        public Task HandleNotificationAsync(
                IGuildUpdate        notification,
                CancellationToken   cancellationToken)
            => SetEntryAsync(
                new(
                    id:     notification.ID,
                    name:   notification.Name,
                    icon:   notification.Icon),
                cancellationToken);

        public async Task HandleNotificationAsync(
            IGuildDelete        notification,
            CancellationToken   cancellationToken)
        {
            using var @lock = await _guildTrackingCache.LockAsync(cancellationToken);

            GuildTrackingLogMessages.GuildUnTracking(_logger, notification.GuildID);
            _guildTrackingCache.RemoveEntry(notification.GuildID);
            GuildTrackingLogMessages.GuildUnTracked(_logger, notification.GuildID);
        }

        private async Task SetEntryAsync(
            GuildTrackingCacheEntry entry,
            CancellationToken       cancellationToken)
        {
            using var @lock = await _guildTrackingCache.LockAsync(cancellationToken);

            GuildTrackingLogMessages.GuildTracking(_logger, entry);
            _guildTrackingCache.SetEntry(entry);
            GuildTrackingLogMessages.GuildTracked(_logger, entry);
        }

        private readonly IGuildTrackingCache    _guildTrackingCache;
        private readonly ILogger                _logger;
    }
}
