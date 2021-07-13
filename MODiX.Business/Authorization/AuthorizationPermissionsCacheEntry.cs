using System.Collections.Generic;

using Remora.Discord.Core;

namespace Modix.Business.Authorization
{
    public class AuthorizationPermissionsCacheEntry
    {
        public AuthorizationPermissionsCacheEntry(
            Snowflake                   guildId,
            Snowflake                   userId,
            IReadOnlyCollection<int>    grantedPermissionIds)
        {
            GuildId                 = guildId;
            UserId                  = userId;
            GrantedPermissionIds    = grantedPermissionIds;
        }

        public Snowflake GuildId { get; }

        public Snowflake UserId { get; }

        public IReadOnlyCollection<int> GrantedPermissionIds { get; }
    }
}
