using Remora.Discord.API.Abstractions.Objects;
using Remora.Discord.Core;

namespace Modix.Business.Guilds.Tracking
{
    public record GuildTrackingCacheEntry
    {
        public GuildTrackingCacheEntry(
            Snowflake   id,
            string      name,
            IImageHash? icon)
        {
            Id      = id;
            Name    = name;
            Icon    = icon;
        }

        public Snowflake Id { get; init; }

        public string Name { get; init; }

        public IImageHash? Icon { get; init; }
    }
}
