using System;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Remora.Discord.API.Abstractions.Gateway.Events;

namespace Modix.Business.Guilds.Tracking
{
    public class GuildTrackingEventListeningBehavior
        : ReactiveBehaviorBase
    {
        public GuildTrackingEventListeningBehavior(
                    IObservable<IGuildCreate>                       guildCreated,
                    IObservable<IGuildDelete>                       guildDeleted,
                    IGuildTrackingCache                             guildTrackingCache,
                    IObservable<IGuildUpdate>                       guildUpdated,
                    ILogger<GuildTrackingEventListeningBehavior>    logger)
                : base(logger)
            => _behavior = Observable.Merge(
                Observable.Merge(
                        guildCreated
                            .Select(@event => new GuildTrackingCacheEntry(
                                id:     @event.ID,
                                name:   @event.Name,
                                icon:   @event.Icon)),
                        guildUpdated
                            .Select(@event => new GuildTrackingCacheEntry(
                                id:     @event.ID,
                                name:   @event.Name,
                                icon:   @event.Icon)))
                    .Select(entry => Observable.FromAsync(async (cancellationToken) =>
                    {
                        using var @lock = await guildTrackingCache.LockAsync(cancellationToken);

                        GuildTrackingLogMessages.GuildTracking(Logger, entry);
                        guildTrackingCache.SetEntry(entry);
                        GuildTrackingLogMessages.GuildTracked(Logger, entry);
                    }))
                    .Switch()
                    .Select(_ => Unit.Default),
                guildDeleted
                    .Select(@event => Observable.FromAsync(async (cancellationToken) => 
                    {
                        using var @lock = await guildTrackingCache.LockAsync(cancellationToken);

                        GuildTrackingLogMessages.GuildUnTracking(Logger, @event.GuildID);
                        guildTrackingCache.RemoveEntry(@event.GuildID);
                        GuildTrackingLogMessages.GuildUnTracked(Logger, @event.GuildID);
                    }))
                    .Switch()
                    .Select(_ => Unit.Default));

        protected override IDisposable Start(IScheduler scheduler)
            => _behavior
                .SubscribeOn(scheduler)
                .Subscribe();

        private readonly IObservable<Unit> _behavior;
    }
}
