using System.Reactive.PlatformServices;

using Modix.Business.Caching;

using Snowflake = Remora.Discord.Core.Snowflake;

namespace Modix.Business.Users.Tracking
{
    public interface IUserTrackingCache
        : IFifoCache<Snowflake, UserTrackingCacheEntry> { }

    internal class UserTrackingCache
        : FifoCacheBase<Snowflake, UserTrackingCacheEntry>,
            IUserTrackingCache
    {
        public UserTrackingCache(ISystemClock systemClock)
            : base(systemClock) { }

        protected override Snowflake SelectKey(UserTrackingCacheEntry entry)
            => entry.UserId;
    }
}
