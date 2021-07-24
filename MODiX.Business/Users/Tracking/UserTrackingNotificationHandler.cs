using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using Remora.Discord.API.Abstractions.Gateway.Events;
using Remora.Discord.Core;

using Modix.Business.Messaging;

namespace Modix.Business.Users.Tracking
{
    public class UserTrackingNotificationHandler
        : INotificationHandler<IGuildMemberAdd>,
            INotificationHandler<IGuildMemberUpdate>,
            INotificationHandler<IMessageCreate>,
            INotificationHandler<IMessageReactionAdd>,
            INotificationHandler<IMessageUpdate>,
            INotificationHandler<IPresenceUpdate>
    {
        public UserTrackingNotificationHandler(
            ILogger<UserTrackingNotificationHandler>    logger,
            IUserTrackingService                        userTrackingService)
        {
            _logger                 = logger;
            _userTrackingService    = userTrackingService;
        }

        public Task HandleNotificationAsync(
                IGuildMemberAdd     notification,
                CancellationToken   cancellationToken)
            => TrackUserAsync(
                guildId:            notification.GuildID,
                userId:             notification.User.Value!.ID,
                username:           notification.User.Value.Username,
                discriminator:      notification.User.Value.Discriminator,
                avatarHash:         notification.User.Value.Avatar?.Value,
                nickname:           notification.Nickname,
                cancellationToken:  cancellationToken);

        public Task HandleNotificationAsync(
                IGuildMemberUpdate  notification,
                CancellationToken   cancellationToken)
            => TrackUserAsync(
                guildId:            notification.GuildID,
                userId:             notification.User.ID,
                username:           notification.User.Username,
                discriminator:      notification.User.Discriminator,
                avatarHash:         notification.User.Avatar?.Value,
                nickname:           notification.Nickname,
                cancellationToken:  cancellationToken);

        public Task HandleNotificationAsync(
                IMessageCreate      notification,
                CancellationToken   cancellationToken)
            => notification.GuildID.HasValue
                ? TrackUserAsync(
                    guildId:            notification.GuildID.Value,
                    userId:             notification.Author.ID,
                    username:           notification.Author.Username,
                    discriminator:      notification.Author.Discriminator,
                    avatarHash:         notification.Author.Avatar?.Value,
                    nickname:           notification.Member.HasValue
                        ? notification.Member.Value.Nickname
                        : default,
                    cancellationToken:  cancellationToken)
                : Task.CompletedTask;

        public Task HandleNotificationAsync(
                IMessageReactionAdd notification,
                CancellationToken   cancellationToken)
            => (notification.GuildID.HasValue && notification.Member.HasValue)
                ? TrackUserAsync(
                    guildId:            notification.GuildID.Value,
                    userId:             notification.UserID,
                    username:           notification.Member.Value!.User.HasValue
                        ? notification.Member.Value.User.Value.Username
                        : default(Optional<string>),
                    discriminator:      notification.Member.Value.User.HasValue
                        ? notification.Member.Value.User.Value.Discriminator
                        : default(Optional<ushort>),
                    avatarHash:         notification.Member.Value.User.HasValue
                        ? notification.Member.Value.User.Value.Avatar?.Value
                        : default,
                    nickname:           notification.Member.HasValue
                        ? notification.Member.Value.Nickname
                        : default,
                    cancellationToken:  cancellationToken)
                : Task.CompletedTask;

        public Task HandleNotificationAsync(
                IMessageUpdate      notification,
                CancellationToken   cancellationToken)
            => (notification.GuildID.HasValue && notification.Author.HasValue)
                ? TrackUserAsync(
                    guildId:            notification.GuildID.Value,
                    userId:             notification.Author.Value!.ID,
                    username:           notification.Author.Value.Username,
                    discriminator:      notification.Author.Value.Discriminator,
                    avatarHash:         notification.Author.Value.Avatar?.Value,
                    nickname:           notification.Member.HasValue
                        ? notification.Member.Value.Nickname
                        : default,
                    cancellationToken:  cancellationToken)
                : Task.CompletedTask;

        public Task HandleNotificationAsync(
                IPresenceUpdate     notification,
                CancellationToken   cancellationToken)
            => notification.User.ID.HasValue
                ? TrackUserAsync(
                    guildId:            notification.GuildID,
                    userId:             notification.User.ID.Value,
                    username:           notification.User.Username,
                    discriminator:      notification.User.Discriminator,
                    avatarHash:         notification.User.Avatar.HasValue
                        ? notification.User.Avatar.Value?.Value
                        : default,
                    nickname:           default,
                    cancellationToken:  cancellationToken)
                : Task.CompletedTask;

        private async Task TrackUserAsync(
            Snowflake           userId,
            Snowflake           guildId,
            Optional<string>    username,
            Optional<ushort>    discriminator,
            Optional<string?>   avatarHash,
            Optional<string?>   nickname,
            CancellationToken   cancellationToken)
        {
            UserTrackingLogMessages.UserTracking(_logger, userId, guildId, username, discriminator, avatarHash, nickname);
            await _userTrackingService.TrackUserAsync(
                userId:             userId,
                guildId:            guildId,
                username:           username,
                discriminator:      discriminator,
                avatarHash:         avatarHash,
                nickname:           nickname,
                cancellationToken:  cancellationToken);
            UserTrackingLogMessages.UserTracked(_logger, userId, guildId);
        }

        private readonly ILogger                _logger;
        private readonly IUserTrackingService   _userTrackingService;
    }
}
