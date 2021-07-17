using System.Collections.Generic;
using System.Linq;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

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
        public PermissionsRepository(
            ILogger<PermissionsRepository>  logger,
            ModixDbContext                  modixDbContext)
        {
            _logger         = logger;
            _modixDbContext = modixDbContext;
        }

        public IAsyncEnumerable<GuildPermissionMappingDefinitionModel> AsyncEnumerateGuildPermissionMappingDefinitions(
            Optional<Snowflake> guildId,
            Optional<bool> isDeleted)
        {
            PermissionsLogMessages.GuildPermissionMappingsEnumerationBuilding(_logger, guildId, isDeleted);
            var query = _modixDbContext.Set<GuildPermissionMappingEntity>()
                .AsQueryable();

            if (guildId.IsSpecified)
            {
                PermissionsLogMessages.GuildPermissionMappingsEnumerationAddingGuildIdClause(_logger, guildId.Value);
                query = query.Where(gpm => gpm.GuildId == guildId.Value);
            }

            if (isDeleted.IsSpecified)
            {
                PermissionsLogMessages.GuildPermissionMappingsEnumerationAddingIsDeletedClause(_logger, isDeleted.Value);
                query = isDeleted.Value
                    ? query.Where(gpm => gpm.DeletionId != null)
                    : query.Where(gpm => gpm.DeletionId == null);
            }

            PermissionsLogMessages.GuildPermissionMappingsEnumerationFinalizing(_logger);
            var enumeration = query
                .Select(gpm => new GuildPermissionMappingDefinitionModel()
                {
                    PermissionId    = gpm.PermissionId,
                    GuildId         = gpm.GuildId,
                    GuildPermission = gpm.GuildPermission,
                    Type            = gpm.Type
                })
                .AsAsyncEnumerable();

            PermissionsLogMessages.GuildPermissionMappingsEnumerationBuilt(_logger, guildId, isDeleted);
            return enumeration;
        }

        public IAsyncEnumerable<int> AsyncEnumeratePermissionIds()
        {
            PermissionsLogMessages.PermissionIdsEnumerationBuilding(_logger);
            var enumeration = _modixDbContext.Set<PermissionEntity>()
                .Select(p => p.Id)
                .AsAsyncEnumerable();

            PermissionsLogMessages.PermissionIdsEnumerationBuilt(_logger);
            return enumeration;
        }

        public IAsyncEnumerable<RolePermissionMappingDefinitionModel> AsyncEnumerateRolePermissionMappingDefinitions(
            Optional<Snowflake> guildId,
            Optional<bool> isDeleted)
        {
            PermissionsLogMessages.RolePermissionMappingsEnumerationBuilding(_logger, guildId, isDeleted);
            var query = _modixDbContext.Set<RolePermissionMappingEntity>()
                .AsQueryable();

            if (guildId.IsSpecified)
            {
                PermissionsLogMessages.RolePermissionMappingsEnumerationAddingGuildIdClause(_logger, guildId.Value);
                query = query.Where(gpm => gpm.GuildId == guildId.Value);
            }

            if (isDeleted.IsSpecified)
            {
                PermissionsLogMessages.RolePermissionMappingsEnumerationAddingIsDeletedClause(_logger, isDeleted.Value);
                query = isDeleted.Value
                    ? query.Where(gpm => gpm.DeletionId != null)
                    : query.Where(gpm => gpm.DeletionId == null);
            }

            PermissionsLogMessages.RolePermissionMappingsEnumerationFinalizing(_logger);
            var enumeration = query
                .Select(rpm => new RolePermissionMappingDefinitionModel()
                {
                    PermissionId    = rpm.PermissionId,
                    GuildId         = rpm.GuildId,
                    RoleId          = rpm.RoleId,
                    Type            = rpm.Type
                })
                .AsAsyncEnumerable();

            PermissionsLogMessages.RolePermissionMappingsEnumerationBuilt(_logger, guildId, isDeleted);
            return enumeration;
        }

        private readonly ILogger        _logger;
        private readonly ModixDbContext _modixDbContext;
    }
}
