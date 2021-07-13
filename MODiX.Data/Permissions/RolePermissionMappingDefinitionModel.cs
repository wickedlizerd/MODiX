using Remora.Discord.Core;

namespace Modix.Data.Permissions
{
    public class RolePermissionMappingDefinitionModel
    {
        public int PermissionId { get; init; }

        public Snowflake GuildId { get; init; }

        public Snowflake RoleId { get; init; }

        public PermissionMappingType Type { get; init; }
    }
}
