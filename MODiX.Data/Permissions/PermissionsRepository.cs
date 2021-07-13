using System.Collections.Generic;
using System.Linq;

using Microsoft.EntityFrameworkCore;

using Modix.Common.ObjectModel;

using Snowflake = Remora.Discord.Core.Snowflake;

namespace Modix.Data.Permissions
{
    public interface IPermissionsRepository
    {
        IAsyncEnumerable<GuildPermissionMappingDefinitionModel> AsyncEnumerateGuildPermissionMappingDefinitions(
            Optional<Snowflake> guildId,
            Optional<bool> isDeleted);

        IAsyncEnumerable<int> AsyncEnumeratePermissionIds();

        IAsyncEnumerable<RolePermissionMappingDefinitionModel> AsyncEnumerateRolePermissionMappingDefinitions(
            Optional<Snowflake> guildId,
            Optional<bool> isDeleted);
    }

    internal class PermissionsRepository
        : IPermissionsRepository
    {
        public PermissionsRepository(ModixDbContext modixDbContext)
            => _modixDbContext = modixDbContext;

        public IAsyncEnumerable<GuildPermissionMappingDefinitionModel> AsyncEnumerateGuildPermissionMappingDefinitions(
            Optional<Snowflake> guildId,
            Optional<bool> isDeleted)
        {
            var query = _modixDbContext.Set<GuildPermissionMappingEntity>()
                .AsQueryable();

            if (guildId.IsSpecified)
                query = query.Where(gpm => gpm.GuildId == guildId.Value);

            if (isDeleted.IsSpecified)
                query = isDeleted.Value
                    ? query.Where(gpm => gpm.DeletionId != null)
                    : query.Where(gpm => gpm.DeletionId == null);

            return query
                .Select(gpm => new GuildPermissionMappingDefinitionModel()
                {
                    PermissionId    = gpm.PermissionId,
                    GuildId         = gpm.GuildId,
                    GuildPermission = gpm.GuildPermission,
                    Type            = gpm.Type
                })
                .AsAsyncEnumerable();
        }

        public IAsyncEnumerable<int> AsyncEnumeratePermissionIds()
            => _modixDbContext.Set<PermissionEntity>()
                .Select(p => p.Id)
                .AsAsyncEnumerable();

        public IAsyncEnumerable<RolePermissionMappingDefinitionModel> AsyncEnumerateRolePermissionMappingDefinitions(
            Optional<Snowflake> guildId,
            Optional<bool> isDeleted)
        {
            var query = _modixDbContext.Set<RolePermissionMappingEntity>()
                .AsQueryable();

            if (guildId.IsSpecified)
                query = query.Where(gpm => gpm.GuildId == guildId.Value);

            if (isDeleted.IsSpecified)
                query = isDeleted.Value
                    ? query.Where(gpm => gpm.DeletionId != null)
                    : query.Where(gpm => gpm.DeletionId == null);

            return query
                .Select(rpm => new RolePermissionMappingDefinitionModel()
                {
                    PermissionId    = rpm.PermissionId,
                    GuildId         = rpm.GuildId,
                    RoleId          = rpm.RoleId,
                    Type            = rpm.Type
                })
                .AsAsyncEnumerable();
        }

        private readonly ModixDbContext _modixDbContext;
    }
}
