using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.PlatformServices;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Modix.Data.Users;

namespace Modix.Business.Users.Tracking
{
    internal class UserTrackingCacheCleaningBehavior
        : BackgroundService
    {
        public UserTrackingCacheCleaningBehavior(
            ILogger<UserTrackingCacheCleaningBehavior>  logger,
            IServiceScopeFactory                        serviceScopeFactory,
            ISystemClock                                systemClock,
            IUserTrackingCache                          userTrackingCache,
            IOptions<UserTrackingConfiguration>         userTrackingConfiguration)
        {
            _logger                     = logger;
            _serviceScopeFactory        = serviceScopeFactory;
            _systemClock                = systemClock;
            _userTrackingCache          = userTrackingCache;
            _userTrackingConfiguration  = userTrackingConfiguration;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // Each time the oldest entry in the cache changes (and isn't null), wait until CacheTimeout after it was added, then run a cleanup.
            var timer = _userTrackingCache.OldestEntryAdded
                .WhereNotNull()
                .Delay(oldestEntryAdded => Observable.Timer(dueTime: oldestEntryAdded + (_userTrackingConfiguration.Value.CacheTimeout ?? UserTrackingDefaults.DefaultCacheTimeout)))
                .Publish();

            var whenStopped = ListenAsync();

            using var timerConnection = timer.Connect();
            stoppingToken.Register(() => timerConnection.Dispose());

            await whenStopped;

            async Task ListenAsync()
            {
                while(!stoppingToken.IsCancellationRequested)
                {
                    await timer.FirstAsync();

                    using var serviceScope = _serviceScopeFactory.CreateScope();
                    using var logScope = UserTrackingLogMessages.BeginBackgroundScope(_logger, Guid.NewGuid());

                    UserTrackingLogMessages.CacheCleaning(_logger);

                    var now = _systemClock.UtcNow;

                    // Retrieve all expired models from the cache, and pick out the ones that have been updated since the last save.
                    // For those, we will perform a save, and re-add them to the cache, with a new LastSave timestamp.
                    // The rest will be discarded.

                    IReadOnlyList<UserTrackingCacheEntry> entriesToSave;
                    using (var @lock = await _userTrackingCache.LockAsync(stoppingToken))
                    {
                        var timeout = _userTrackingConfiguration.Value.CacheTimeout ?? UserTrackingDefaults.DefaultCacheTimeout;

                        UserTrackingLogMessages.CacheEntriesRemoving(_logger, timeout);
                        entriesToSave = _userTrackingCache.RemoveOldEntries(timeout)
                            .Where(trackingModel => trackingModel.LastUpdated > trackingModel.LastSaved)
                            .ToArray();
                        UserTrackingLogMessages.CacheEntriesRemoved(_logger);

                        if (entriesToSave.Count is 0)
                            UserTrackingLogMessages.CacheEntriesResetNotNeeded(_logger);
                        else
                        {
                            UserTrackingLogMessages.CacheEntriesResetting(_logger, entriesToSave.Count);
                            foreach (var entry in entriesToSave)
                                _userTrackingCache.SetEntry(entry with
                                {
                                    LastSaved = now
                                });
                            UserTrackingLogMessages.CacheEntriesReset(_logger, entriesToSave.Count);
                        }
                    }

                    if (entriesToSave.Count is 0)
                        UserTrackingLogMessages.CacheEntriesSaveNotNeeded(_logger);
                    else
                    {
                        UserTrackingLogMessages.CacheEntriesSaving(_logger, entriesToSave.Count);
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
                                stoppingToken);
                        UserTrackingLogMessages.CacheEntriesSaved(_logger, entriesToSave.Count);
                    }

                    UserTrackingLogMessages.CacheCleaned(_logger);
                }
            }
        }

        private readonly ILogger                                _logger;
        private readonly IServiceScopeFactory                   _serviceScopeFactory;
        private readonly ISystemClock                           _systemClock;
        private readonly IUserTrackingCache                     _userTrackingCache;
        private readonly IOptions<UserTrackingConfiguration>    _userTrackingConfiguration;
    }
}
