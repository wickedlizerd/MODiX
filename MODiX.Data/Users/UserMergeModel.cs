using System;

using Remora.Discord.Core;

namespace Modix.Data.Users
{
    public class UserMergeModel
    {
        public UserMergeModel(
            Snowflake           guildId,
            Snowflake           userId,
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

        public Snowflake GuildId { get; }

        public Snowflake UserId { get; }

        public Optional<string> Username { get; }

        public Optional<ushort> Discriminator { get; }

        public Optional<string?> AvatarHash { get; }

        public Optional<string?> Nickname { get; }

        public DateTimeOffset Timestamp { get; }
    }
}
