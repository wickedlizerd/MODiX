using System.Reactive.PlatformServices;

using Microsoft.Extensions.Logging;

using Remora.Discord.Core;

using Modix.Business.Caching;

namespace Modix.Business.Users.Tracking
{
    public interface IUserTrackingCache
        : IFifoCache<Snowflake, UserTrackingCacheEntry> { }

    internal class UserTrackingCache
        : FifoCacheBase<Snowflake, UserTrackingCacheEntry>,
            IUserTrackingCache
    {
        public UserTrackingCache(
                ILogger<UserTrackingCache>  logger,
                ISystemClock                systemClock)
            : base(
                logger,
                systemClock) { }

        protected override Snowflake SelectKey(UserTrackingCacheEntry entry)
            => entry.UserId;
    }
}
