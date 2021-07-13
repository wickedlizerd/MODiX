using Remora.Discord.API.Abstractions.Objects;
using Remora.Discord.Core;

namespace Modix.Data.Permissions
{
    public class GuildPermissionMappingDefinitionModel
    {
        public int PermissionId { get; init; }

        public Snowflake GuildId { get; init; }

        public DiscordPermission GuildPermission { get; init; }

        public PermissionMappingType Type { get; init; }
    }
}
