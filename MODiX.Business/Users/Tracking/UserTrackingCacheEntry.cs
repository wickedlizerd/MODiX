using System;
using System.Collections.Immutable;

using Modix.Common.ObjectModel;

using Snowflake = Remora.Discord.Core.Snowflake;

namespace Modix.Business.Users.Tracking
{
    public record UserTrackingCacheEntry
    {
        public UserTrackingCacheEntry(
            Snowflake                               userId,
            Optional<string>                        username,
            Optional<ushort>                        discriminator,
            Optional<string?>                       avatarHash,
            ImmutableDictionary<Snowflake, string?> nicknamesByGuildId,
            DateTimeOffset                          lastUpdated,
            DateTimeOffset                          lastSaved)
        {
            UserId              = userId;
            Username            = username;
            Discriminator       = discriminator;
            AvatarHash          = avatarHash;
            NicknamesByGuildId  = nicknamesByGuildId;
            LastUpdated         = lastUpdated;
            LastSaved           = lastSaved;
        }

        public Snowflake UserId { get; init; }

        public Optional<string> Username { get; init; }

        public Optional<ushort> Discriminator { get; init; }

        public Optional<string?> AvatarHash { get; init; }

        public ImmutableDictionary<Snowflake, string?> NicknamesByGuildId { get; init; }

        public DateTimeOffset LastUpdated { get; init; }

        public DateTimeOffset LastSaved { get; init; }
    }
}
