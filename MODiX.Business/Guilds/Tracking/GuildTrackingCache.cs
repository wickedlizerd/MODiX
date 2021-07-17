using Remora.Discord.Core;

using Microsoft.Extensions.Logging;

using Modix.Business.Caching;

namespace Modix.Business.Guilds.Tracking
{
    public interface IGuildTrackingCache
        : IPersistentCache<Snowflake, GuildTrackingCacheEntry> { }

    internal class GuildTrackingCache
        : PersistentCacheBase<Snowflake, GuildTrackingCacheEntry>,
            IGuildTrackingCache
    {
        public GuildTrackingCache(ILogger<GuildTrackingCache> logger)
            : base(logger) { }

        protected override Snowflake SelectKey(GuildTrackingCacheEntry entry)
            => entry.Id;
    }
}
