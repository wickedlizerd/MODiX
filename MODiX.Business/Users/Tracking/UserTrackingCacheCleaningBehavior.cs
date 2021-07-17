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
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Modix.Data.Users;

namespace Modix.Business.Users.Tracking
{
    internal class UserTrackingCacheCleaningBehavior
        : ReactiveBehaviorBase
    {
        public UserTrackingCacheCleaningBehavior(
                    ILogger<UserTrackingCacheCleaningBehavior>  logger,
                    IServiceScopeFactory                        serviceScopeFactory,
                    ISystemClock                                systemClock,
                    IUserTrackingCache                          userTrackingCache,
                    IOptions<UserTrackingConfiguration>         userTrackingConfiguration)
                : base(logger)
            // Each time the oldest entry in the cache changes (and isn't null), wait until CacheTimeout after it was added, then run a cleanup.
            => _behavior = userTrackingCache.OldestEntryAdded
                .WhereNotNull()
                .Delay(oldestEntryAdded => Observable.Timer(dueTime: oldestEntryAdded + (userTrackingConfiguration.Value.CacheTimeout ?? UserTrackingDefaults.DefaultCacheTimeout)))
                .Select(_ => Observable.FromAsync(async () =>
                {
                    UserTrackingLogMessages.CacheCleaning(Logger);

                    using var serviceScope = serviceScopeFactory.CreateScope();

                    var now = systemClock.UtcNow;

                    // Retrieve all expired models from the cache, and pick out the ones that have been updated since the last save.
                    // For those, we will perform a save, and re-add them to the cache, with a new LastSave timestamp.
                    // The rest will be discarded.

                    IReadOnlyList<UserTrackingCacheEntry> entriesToSave;
                    using (var @lock = await userTrackingCache.LockAsync(CancellationToken.None))
                    {
                        var timeout = userTrackingConfiguration.Value.CacheTimeout ?? UserTrackingDefaults.DefaultCacheTimeout;

                        UserTrackingLogMessages.CacheEntriesRemoving(Logger, timeout);
                        entriesToSave = userTrackingCache.RemoveOldEntries(timeout)
                            .Where(trackingModel => trackingModel.LastUpdated > trackingModel.LastSaved)
                            .ToArray();
                        UserTrackingLogMessages.CacheEntriesRemoved(Logger);

                        if (entriesToSave.Count is 0)
                            UserTrackingLogMessages.CacheEntriesResetNotNeeded(Logger);
                        else
                        {
                            UserTrackingLogMessages.CacheEntriesResetting(Logger, entriesToSave.Count);
                            foreach (var entry in entriesToSave)
                                userTrackingCache.SetEntry(entry with
                                {
                                    LastSaved = now
                                });
                            UserTrackingLogMessages.CacheEntriesReset(Logger, entriesToSave.Count);
                        }
                    }

                    if (entriesToSave.Count is 0)
                        UserTrackingLogMessages.CacheEntriesSaveNotNeeded(Logger);
                    else
                    {
                        UserTrackingLogMessages.CacheEntriesSaving(Logger, entriesToSave.Count);
                        await serviceScope.ServiceProvider.GetRequiredService<IUsersRepository>()
                            .MergeAsync(
                                entriesToSave
                                    .SelectMany(entry => entry.NicknamesByGuildId
                                        .Select(nicknameByGuildId => new UserMergeModel(
                                            guildId:        nicknameByGuildId.Key,
                                            userId:         entry.UserId,
                                            username:       entry.Username,
                                            discriminator:  entry.Discriminator,
                                            avatarHash:     entry.AvatarHash,
                                            nickname:       nicknameByGuildId.Value,
                                            timestamp:      entry.LastUpdated))),
                                CancellationToken.None);
                        UserTrackingLogMessages.CacheEntriesSaved(Logger, entriesToSave.Count);
                    }

                    UserTrackingLogMessages.CacheCleaned(Logger);
                }))
                .Switch();

        protected override IDisposable Start(IScheduler scheduler)
            => _behavior
                .SubscribeOn(scheduler)
                .Subscribe();

        private readonly IObservable<Unit> _behavior;
    }
}
