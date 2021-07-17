using System;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Remora.Discord.API.Abstractions.Gateway.Events;

using Modix.Common.ObjectModel;

using Snowflake = Remora.Discord.Core.Snowflake;

namespace Modix.Business.Users.Tracking
{
    public class UserTrackingEventListeningBehavior
        : ReactiveBehaviorBase
    {
        public UserTrackingEventListeningBehavior(
                IObservable<IGuildMemberAdd>                guildMemberAdded,
                IObservable<IGuildMemberUpdate>             guildMemberUpdated,
                ILogger<UserTrackingEventListeningBehavior> logger,
                IObservable<IMessageCreate>                 messageCreated,
                IObservable<IMessageReactionAdd>            messageReactionAdded,
                IObservable<IMessageUpdate>                 messageUpdated,
                IObservable<IPresenceUpdate>                presenceUpdated,
                IServiceScopeFactory                        serviceScopeFactory)
            : base(logger)
        {
            _behavior = Observable.Merge(
                    guildMemberAdded
                        .Where(@event => @event.User.HasValue)
                        .SelectMany(@event => TrackUserAsync(
                            guildId:            @event.GuildID,
                            userId:             @event.User.Value!.ID,
                            username:           @event.User.Value.Username,
                            discriminator:      @event.User.Value.Discriminator,
                            avatarHash:         @event.User.Value.Avatar?.Value,
                            nickname:           @event.Nickname)),
                    guildMemberUpdated
                        .SelectMany(@event => TrackUserAsync(
                            guildId:            @event.GuildID,
                            userId:             @event.User.ID,
                            username:           @event.User.Username,
                            discriminator:      @event.User.Discriminator,
                            avatarHash:         @event.User.Avatar?.Value,
                            nickname:           @event.Nickname)),
                    messageCreated
                        .Where(@event => @event.GuildID.HasValue)
                        .SelectMany(@event => TrackUserAsync(
                            guildId:            @event.GuildID.Value,
                            userId:             @event.Author.ID,
                            username:           @event.Author.Username,
                            discriminator:      @event.Author.Discriminator,
                            avatarHash:         @event.Author.Avatar?.Value,
                            nickname:           @event.Member.HasValue
                                ? @event.Member.Value.Nickname
                                : default)),
                    messageReactionAdded
                        .Where(@event => @event.GuildID.HasValue && @event.Member.HasValue)
                        .SelectMany(@event => TrackUserAsync(
                            guildId:            @event.GuildID.Value,
                            userId:             @event.UserID,
                            username:           @event.Member.Value!.User.HasValue
                                ? @event.Member.Value.User.Value.Username
                                : Optional.Unspecified<string>(),
                            discriminator:      @event.Member.Value.User.HasValue
                                ? @event.Member.Value.User.Value.Discriminator
                                : Optional.Unspecified<ushort>(),
                            avatarHash:         @event.Member.Value.User.HasValue
                                ? @event.Member.Value.User.Value.Avatar?.Value
                                : Optional.Unspecified<string?>(),
                            nickname:           @event.Member.HasValue
                                ? @event.Member.Value.Nickname
                                : Optional.Unspecified<string?>())),
                    messageUpdated
                        .Where(@event => @event.GuildID.HasValue && @event.Author.HasValue)
                        .SelectMany(@event => TrackUserAsync(
                            guildId:            @event.GuildID.Value,
                            userId:             @event.Author.Value!.ID,
                            username:           @event.Author.Value.Username,
                            discriminator:      @event.Author.Value.Discriminator,
                            avatarHash:         @event.Author.Value.Avatar?.Value,
                            nickname:           @event.Member.HasValue
                                ? @event.Member.Value.Nickname
                                : Optional.Unspecified<string?>())),
                    presenceUpdated
                        .Where(@event => @event.User.ID.HasValue)
                        .SelectMany(@event => TrackUserAsync(
                            guildId:            @event.GuildID,
                            userId:             @event.User.ID.Value,
                            username:           @event.User.Username,
                            discriminator:      @event.User.Discriminator,
                            avatarHash:         @event.User.Avatar.HasValue
                                ? @event.User.Avatar.Value?.Value
                                : default,
                            nickname:           default)));

            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override IDisposable Start(IScheduler scheduler)
            => _behavior
                .SubscribeOn(scheduler)
                .Subscribe();

        private async ValueTask TrackUserAsync(
            Snowflake           userId,
            Snowflake           guildId,
            Optional<string>    username,
            Optional<ushort>    discriminator,
            Optional<string?>   avatarHash,
            Optional<string?>   nickname)
        {
            using var serviceScope = _serviceScopeFactory.CreateScope();

            UserTrackingLogMessages.UserTracking(Logger, guildId, userId, username, discriminator, avatarHash, nickname);
            await serviceScope.ServiceProvider.GetRequiredService<IUserTrackingService>().TrackUserAsync(
                guildId:            guildId,
                userId:             userId,
                username:           username,
                discriminator:      discriminator,
                avatarHash:         avatarHash,
                nickname:           nickname,
                cancellationToken:  CancellationToken.None);
            UserTrackingLogMessages.UserTracked(Logger, guildId, userId);
        }

        private readonly IObservable<Unit>      _behavior;
        private readonly IServiceScopeFactory   _serviceScopeFactory;
    }
}
