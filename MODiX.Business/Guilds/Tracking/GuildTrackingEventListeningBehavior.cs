using System;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;

using Microsoft.Extensions.Hosting;

using Remora.Discord.API.Abstractions.Gateway.Events;

namespace Modix.Business.Guilds.Tracking
{
    public class GuildTrackingEventListeningBehavior
        : ReactiveBehaviorBase
    {
        public GuildTrackingEventListeningBehavior(
                IObservable<IGuildCreate>   guildCreated,
                IObservable<IGuildDelete>   guildDeleted,
                IGuildTrackingCache         guildTrackingCache,
                IObservable<IGuildUpdate>   guildUpdated)
            => _behavior = Observable.Merge(
                guildCreated
                    .Do(@event => guildTrackingCache.SetEntry(new GuildTrackingCacheEntry(
                        id:     @event.ID,
                        name:   @event.Name,
                        icon:   @event.Icon)))
                    .Select(_ => Unit.Default),
                guildUpdated
                    .Do(@event => guildTrackingCache.SetEntry(new GuildTrackingCacheEntry(
                        id:     @event.ID,
                        name:   @event.Name,
                        icon:   @event.Icon)))
                    .Select(_ => Unit.Default),
                guildDeleted
                    .Do(@event => guildTrackingCache.RemoveEntry(@event.GuildID))
                    .Select(_ => Unit.Default));

        protected override IDisposable Start(IScheduler scheduler)
            => _behavior
                .SubscribeOn(scheduler)
                .Subscribe();

        private readonly IObservable<Unit> _behavior;
    }
}
