using System;
using System.Collections.Immutable;

using Modix.Common.ObjectModel;

namespace Modix.Business.Users.Tracking
{
    internal record UserTrackingModel
    {
        public UserTrackingModel(
            ulong                               userId,
            Optional<string>                    username,
            Optional<ushort>                    discriminator,
            Optional<string?>                   avatarHash,
            ImmutableDictionary<ulong, string?> nicknamesByGuildId,
            DateTimeOffset                      lastUpdated,
            DateTimeOffset                      lastSaved)
        {
            UserId              = userId;
            Username            = username;
            Discriminator       = discriminator;
            AvatarHash          = avatarHash;
            NicknamesByGuildId  = nicknamesByGuildId;
            LastUpdated         = lastUpdated;
            LastSaved           = lastSaved;
        }

        public ulong UserId { get; init; }

        public Optional<string> Username { get; init; }

        public Optional<ushort> Discriminator { get; init; }

        public Optional<string?> AvatarHash { get; init; }

        public ImmutableDictionary<ulong, string?> NicknamesByGuildId { get; init; }

        public DateTimeOffset LastUpdated { get; init; }

        public DateTimeOffset LastSaved { get; init; }
    }
}
