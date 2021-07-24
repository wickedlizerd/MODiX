using System.Collections.Generic;

using Remora.Discord.Core;

namespace Modix.Business.Authorization
{
    public class AuthorizationPermissionsCacheEntry
    {
        public AuthorizationPermissionsCacheEntry(
            Snowflake                   userId,
            Snowflake                   guildId,
            IReadOnlyCollection<int>    grantedPermissionIds)
        {
            UserId                  = userId;
            GuildId                 = guildId;
            GrantedPermissionIds    = grantedPermissionIds;
        }

        public Snowflake UserId { get; }

        public Snowflake GuildId { get; }

        public IReadOnlyCollection<int> GrantedPermissionIds { get; }
    }
}
