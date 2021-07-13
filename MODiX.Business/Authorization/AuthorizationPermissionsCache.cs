using System.Reactive.PlatformServices;

using Remora.Discord.Core;

using Modix.Business.Caching;

namespace Modix.Business.Authorization
{
    public interface IAuthorizationPermissionsCache
        : IFifoCache<(Snowflake guildId, Snowflake userId), AuthorizationPermissionsCacheEntry> { }

    internal class AuthorizationPermissionsCache
        : FifoCacheBase<(Snowflake guildId, Snowflake userId), AuthorizationPermissionsCacheEntry>,
            IAuthorizationPermissionsCache
    {
        public AuthorizationPermissionsCache(ISystemClock systemClock)
            : base(systemClock) { }

        protected override (Snowflake guildId, Snowflake userId) SelectKey(AuthorizationPermissionsCacheEntry entry)
            => (entry.GuildId, entry.UserId);
    }
}
