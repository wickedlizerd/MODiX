using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.PlatformServices;
using System.Threading;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

using Modix.Data.Users;

namespace Modix.Business.Users.Tracking
{
    internal class UserTrackingCacheCleaningBehavior
        : ReactiveBehaviorBase
    {
        public UserTrackingCacheCleaningBehavior(
                IServiceScopeFactory                serviceScopeFactory,
                ISystemClock                        systemClock,
                IUserTrackingCache                  userTrackingCache,
                IOptions<UserTrackingConfiguration> userTrackingConfiguration)
            // Each time the oldest entry in the cache changes (and isn't null), wait until CacheTimeout after it was added, then run a cleanup.
            => _behavior = userTrackingCache.OldestEntryAdded
                .WhereNotNull()
                .Delay(oldestEntryAdded => Observable.Timer(dueTime: oldestEntryAdded + (userTrackingConfiguration.Value.CacheTimeout ?? UserTrackingDefaults.DefaultCacheTimeout)))
                .SelectMany(async _ =>
                {
                    using var serviceScope = serviceScopeFactory.CreateScope();

                    var now = systemClock.UtcNow;

                    // Retrieve all expired models from the cache, and pick out the ones that have been updated since the last save.
                    // For those, we will perform a save, and re-add them to the cache, with a new LastSave timestamp.
                    // The rest will be discarded.

                    IReadOnlyList<UserTrackingCacheEntry> trackingModelsToSave;
                    using (var @lock = await userTrackingCache.LockAsync(CancellationToken.None))
                    {
                        trackingModelsToSave = userTrackingCache.RemoveOldEntries(userTrackingConfiguration.Value.CacheTimeout ?? UserTrackingDefaults.DefaultCacheTimeout)
                            .Where(trackingModel => trackingModel.LastUpdated > trackingModel.LastSaved)
                            .ToArray();

                        foreach(var trackingModel in trackingModelsToSave)
                            userTrackingCache.SetEntry(trackingModel with
                            {
                                LastSaved = now
                            });
                    }

                    if (trackingModelsToSave.Count is not 0)
                        await serviceScope.ServiceProvider.GetRequiredService<IUsersRepository>()
                            .MergeAsync(
                                trackingModelsToSave
                                    .SelectMany(trackingModel => trackingModel.NicknamesByGuildId
                                        .Select(nicknameByGuildId => new UserMergeModel(
                                            guildId:        nicknameByGuildId.Key,
                                            userId:         trackingModel.UserId,
                                            username:       trackingModel.Username,
                                            discriminator:  trackingModel.Discriminator,
                                            avatarHash:     trackingModel.AvatarHash,
                                            nickname:       nicknameByGuildId.Value,
                                            timestamp:      trackingModel.LastUpdated))),
                                CancellationToken.None);
                });

        protected override IDisposable Start(IScheduler scheduler)
            => _behavior
                .SubscribeOn(scheduler)
                .Subscribe();

        private readonly IObservable<Unit> _behavior;
    }
}
