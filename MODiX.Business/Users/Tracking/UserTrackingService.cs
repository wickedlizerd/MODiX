using System.Collections.Immutable;
using System.Reactive.PlatformServices;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using Remora.Discord.Core;

using Modix.Data.Users;

namespace Modix.Business.Users.Tracking
{
    public interface IUserTrackingService
    {
        ValueTask TrackUserAsync(
            Snowflake           userId,
            Snowflake           guildId,
            Optional<string>    username,
            Optional<ushort>    discriminator,
            Optional<string?>   avatarHash,
            Optional<string?>   nickname,
            CancellationToken   cancellationToken);
    }

    internal class UserTrackingService
        : IUserTrackingService
    {
        public UserTrackingService(
            ILogger<UserTrackingService>    logger,
            ISystemClock                    systemClock,
            IUsersRepository                usersRepository,
            IUserTrackingCache              userTrackingCache)
        {
            _logger             = logger;
            _systemClock        = systemClock;
            _usersRepository    = usersRepository;
            _userTrackingCache  = userTrackingCache;
        }

        public async ValueTask TrackUserAsync(
            Snowflake           userId,
            Snowflake           guildId,
            Optional<string>    username,
            Optional<ushort>    discriminator,
            Optional<string?>   avatarHash,
            Optional<string?>   nickname,
            CancellationToken   cancellationToken)
        {
            UserTrackingLogMessages.UserTracking(_logger, userId, guildId, username, discriminator, avatarHash, nickname);

            var now = _systemClock.UtcNow;
            bool isSaveNeeded;

            // Check the cache to see what's changed since the last save.
            // If only the timestamp has changed, just update the existing cache entry, in place, and leave the LastSaved timestamp.
            // If anything else has changed, remove and re-add the cache entry to move it to the back of the queue, and update the LastSaved timestamp to now.
            // Then actually perform a save.

            using (var @lock = await _userTrackingCache.LockAsync(cancellationToken))
            {
                UserTrackingLogMessages.CacheEntryRetrieving(_logger, userId);
                var currentEntry = _userTrackingCache.TryGetEntry(userId);
                if (currentEntry is null)
                {
                    UserTrackingLogMessages.CacheEntryNotFound(_logger, userId);
                    isSaveNeeded = true;
                }
                else
                {
                    UserTrackingLogMessages.CacheEntryRetrieved(_logger, currentEntry);
                    isSaveNeeded = (username.HasValue && (currentEntry.Username != username))
                        || (discriminator.HasValue && (currentEntry.Discriminator != discriminator))
                        || (avatarHash.HasValue && (currentEntry.AvatarHash != avatarHash))
                        || (nickname.HasValue && !currentEntry.NicknamesByGuildId.Contains(new(guildId, nickname.Value)));
                }

                var nicknamesByGuildId = currentEntry?.NicknamesByGuildId ?? ImmutableDictionary<Snowflake, string?>.Empty;

                var newEntry = new UserTrackingCacheEntry(
                    userId:             userId,
                    username:           username.HasValue
                        ? username
                        : (currentEntry?.Username ?? default),
                    discriminator:      discriminator.HasValue
                        ? discriminator
                        : (currentEntry?.Discriminator ?? default),
                    avatarHash:         avatarHash.HasValue
                        ? avatarHash
                        : (currentEntry?.AvatarHash ?? default),
                    lastUpdated:        now,
                    nicknamesByGuildId: nickname.HasValue
                        ? nicknamesByGuildId.SetItem(guildId, nickname.Value)
                        : nicknamesByGuildId,
                    lastSaved:          isSaveNeeded
                        ? now
                        : (currentEntry?.LastSaved ?? now));

                if (currentEntry is null)
                {
                    UserTrackingLogMessages.CacheEntryAdding(_logger, newEntry);
                    _userTrackingCache.SetEntry(newEntry);
                    UserTrackingLogMessages.CacheEntryAdded(_logger, newEntry);
                }
                else if (isSaveNeeded)
                {
                    // If we're going to save the model, move it to the back of the queue
                    UserTrackingLogMessages.CacheEntryResetting(_logger, currentEntry);
                    _userTrackingCache.RemoveEntry(userId);
                    _userTrackingCache.SetEntry(newEntry);
                    UserTrackingLogMessages.CacheEntryReset(_logger, newEntry);
                }
                else
                {
                    UserTrackingLogMessages.CacheEntryReplacing(_logger, currentEntry);
                    _userTrackingCache.SetEntry(newEntry);
                    UserTrackingLogMessages.CacheEntryReplaced(_logger, newEntry);
                }
            }

            if (isSaveNeeded)
            {
                UserTrackingLogMessages.CacheEntrySaving(_logger, userId, guildId);
                await _usersRepository.MergeAsync(
                    new UserMergeModel(
                        guildId:        guildId,
                        userId:         userId,
                        username:       username,
                        discriminator:  discriminator,
                        avatarHash:     avatarHash,
                        nickname:       nickname,
                        timestamp:      now),
                    cancellationToken:  cancellationToken);
                UserTrackingLogMessages.CacheEntrySaved(_logger, userId, guildId);
            }

            UserTrackingLogMessages.UserTracked(_logger, userId, guildId);
        }

        private readonly ILogger            _logger;
        private readonly ISystemClock       _systemClock;
        private readonly IUsersRepository   _usersRepository;
        private readonly IUserTrackingCache _userTrackingCache;
    }
}
