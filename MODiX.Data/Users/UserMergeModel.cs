using System;

using Modix.Common.ObjectModel;

namespace Modix.Data.Users
{
    public class UserMergeModel
    {
        public UserMergeModel(
            ulong               guildId,
            ulong               userId,
            Optional<string>    username,
            Optional<ushort>    discriminator,
            Optional<string?>   avatarHash,
            Optional<string?>   nickname,
            DateTimeOffset      timestamp)
        {
            GuildId         = guildId;
            UserId          = userId;
            Username        = username;
            Discriminator   = discriminator;
            AvatarHash      = avatarHash;
            Nickname        = nickname;
            Timestamp       = timestamp;
        }

        public ulong GuildId { get; }

        public ulong UserId { get; }

        public Optional<string> Username { get; }

        public Optional<ushort> Discriminator { get; }

        public Optional<string?> AvatarHash { get; }

        public Optional<string?> Nickname { get; }

        public DateTimeOffset Timestamp { get; }
    }
}
