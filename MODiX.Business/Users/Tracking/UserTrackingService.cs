using System.Collections.Immutable;
using System.Reactive.PlatformServices;
using System.Threading;
using System.Threading.Tasks;

using Modix.Common.ObjectModel;
using Modix.Data.Users;

using Snowflake = Remora.Discord.Core.Snowflake;

namespace Modix.Business.Users.Tracking
{
    public interface IUserTrackingService
    {
        ValueTask TrackUserAsync(
            Snowflake           guildId,
            Snowflake           userId,
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
            ISystemClock        systemClock,
            IUsersRepository    usersRepository,
            IUserTrackingCache  userTrackingCache)
        {
            _systemClock        = systemClock;
            _usersRepository    = usersRepository;
            _userTrackingCache  = userTrackingCache;
        }

        public async ValueTask TrackUserAsync(
            Snowflake           guildId,
            Snowflake           userId,
            Optional<string>    username,
            Optional<ushort>    discriminator,
            Optional<string?>   avatarHash,
            Optional<string?>   nickname,
            CancellationToken   cancellationToken)
        {
            var now = _systemClock.UtcNow;
            bool isSaveNeeded;

            // Check the cache to see what's changed since the last save.
            // If only the timestamp has changed, just update the existing cache entry, in place, and leave the LastSaved timestamp.
            // If anything else has changed, remove and re-add the cache entry to move it to the back of the queue, and update the LastSaved timestamp to now.
            // Then actually perform a save.

            using (var @lock = await _userTrackingCache.LockAsync(cancellationToken))
            {
                var currentModel = _userTrackingCache.TryGetEntry(userId);

                isSaveNeeded = (currentModel is null)
                    || (username.IsSpecified && (currentModel.Username != username))
                    || (discriminator.IsSpecified && (currentModel.Discriminator != discriminator))
                    || (avatarHash.IsSpecified && (currentModel.AvatarHash != avatarHash))
                    || (nickname.IsSpecified && !currentModel.NicknamesByGuildId.Contains(new(guildId, nickname.Value)));

                if (isSaveNeeded)
                    _userTrackingCache.RemoveEntry(userId);

                var nicknamesByGuildId = currentModel?.NicknamesByGuildId ?? ImmutableDictionary<Snowflake, string?>.Empty;

                _userTrackingCache.SetEntry(new(
                    userId:             userId,
                    username:           username,
                    discriminator:      discriminator,
                    avatarHash:         avatarHash,
                    lastUpdated:        now,
                    nicknamesByGuildId: nickname.IsSpecified
                        ? nicknamesByGuildId.SetItem(guildId, nickname.Value)
                        : nicknamesByGuildId,
                    lastSaved:          isSaveNeeded ? now : (currentModel?.LastSaved ?? now)));
            }

            if(isSaveNeeded)
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
        }

        private readonly ISystemClock       _systemClock;
        private readonly IUsersRepository   _usersRepository;
        private readonly IUserTrackingCache _userTrackingCache;
    }
}
