using Remora.Discord.Core;

using Modix.Business.Caching;

namespace Modix.Business.Guilds.Tracking
{
    public interface IGuildTrackingCache
        : IPersistentCache<Snowflake, GuildTrackingCacheEntry> { }

    internal class GuildTrackingCache
        : PersistentCacheBase<Snowflake, GuildTrackingCacheEntry>,
            IGuildTrackingCache
    {
        protected override Snowflake SelectKey(GuildTrackingCacheEntry entry)
            => entry.Id;
    }
}
