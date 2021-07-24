using System.Reactive.PlatformServices;

using Microsoft.Extensions.Logging;

using Remora.Discord.Core;

using Modix.Business.Caching;

namespace Modix.Business.Authorization
{
    public interface IAuthorizationPermissionsCache
        : IFifoCache<(Snowflake userId, Snowflake guildId), AuthorizationPermissionsCacheEntry> { }

    internal class AuthorizationPermissionsCache
        : FifoCacheBase<(Snowflake userId, Snowflake guildId), AuthorizationPermissionsCacheEntry>,
            IAuthorizationPermissionsCache
    {
        public AuthorizationPermissionsCache(
                ILogger<AuthorizationPermissionsCache>  logger, 
                ISystemClock                            systemClock)
            : base(
                logger,
                systemClock) { }

        protected override (Snowflake userId, Snowflake guildId) SelectKey(AuthorizationPermissionsCacheEntry entry)
            => (entry.UserId, entry.GuildId);
    }
}
