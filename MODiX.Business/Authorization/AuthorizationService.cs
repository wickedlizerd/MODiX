using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Options;

using Remora.Discord.API.Abstractions.Rest;
using Remora.Discord.Core;
using Remora.Results;

using Modix.Data;
using Modix.Data.Permissions;

namespace Modix.Business.Authorization
{
    public interface IAuthorizationService
    {
        ValueTask<Result<IReadOnlyCollection<int>>> GetGrantedPermissionIdsAsync(
            Snowflake           guildId,
            Snowflake           userId,
            CancellationToken   cancellationToken);
    }

    internal class AuthorizationService
        : IAuthorizationService
    {
        public AuthorizationService(
            IOptions<AuthorizationConfiguration>    authorizationConfiguration,
            IAuthorizationPermissionsCache          authorizationPermissionsCache,
            IDiscordRestGuildAPI                    discordRestGuildAPI,
            IPermissionsRepository                  permissionsRepository)
        {
            _authorizationConfiguration     = authorizationConfiguration;
            _authorizationPermissionsCache  = authorizationPermissionsCache;
            _discordRestGuildAPI            = discordRestGuildAPI;
            _permissionsRepository          = permissionsRepository;
        }

        public async ValueTask<Result<IReadOnlyCollection<int>>> GetGrantedPermissionIdsAsync(
            Snowflake           guildId,
            Snowflake           userId,
            CancellationToken   cancellationToken)
        {
            using var @lock = await _authorizationPermissionsCache.LockAsync(cancellationToken);

            // If the user is a hard-coded admin, they get all permissions, period.
            if (_authorizationConfiguration.Value.AdminUserIds.Contains(userId.Value))
            {
                if (_allPermissions is null)
                    _allPermissions = await _permissionsRepository.AsyncEnumeratePermissionIds()
                        .ToHashSetAsync(cancellationToken);

                return Result<IReadOnlyCollection<int>>.FromSuccess(_allPermissions);
            }    

            // If the permissions for this users are cached, return those.
            var cacheEntry = _authorizationPermissionsCache.TryGetEntry((guildId, userId));
            if (cacheEntry is not null)
                return Result<IReadOnlyCollection<int>>.FromSuccess(cacheEntry.GrantedPermissionIds);

            var guildMemberResult = await _discordRestGuildAPI.GetGuildMemberAsync(guildId, userId, cancellationToken);
            if (!guildMemberResult.IsSuccess)
                return Result<IReadOnlyCollection<int>>.FromError(guildMemberResult.Error);

            var guildMember = guildMemberResult.Entity;
            if (!guildMember.Permissions.HasValue)
                return Result<IReadOnlyCollection<int>>.FromError(new DataNotFoundError($"Guild {guildId} permissions for user {userId}"));

            var grantedPermissionIds = new HashSet<int>();

            // Map out permissions from the user's Guild Permissions
            var guildPermissionMappings = _permissionsRepository.AsyncEnumerateGuildPermissionMappingDefinitions(
                guildId:    guildId,
                isDeleted:  false);
            await foreach(var mapping in guildPermissionMappings)
                if (guildMember.Permissions.Value.HasPermission(mapping.GuildPermission))
                {
                    if (mapping.Type == PermissionMappingType.Grant)
                        grantedPermissionIds.Add(mapping.PermissionId);
                    else
                        grantedPermissionIds.Remove(mapping.PermissionId);
                }

            // Map out permissions from the user's Guild Roles, overwriting mappings from above, if applicable.
            var rolePermissionMappings = _permissionsRepository.AsyncEnumerateRolePermissionMappingDefinitions(
                guildId:    guildId,
                isDeleted:  false);
            await foreach (var mapping in rolePermissionMappings)
                if (guildMember.Roles.Contains(mapping.RoleId))
                {
                    if (mapping.Type == PermissionMappingType.Grant)
                        grantedPermissionIds.Add(mapping.PermissionId);
                    else
                        grantedPermissionIds.Remove(mapping.PermissionId);
                }

            // Don't forget to cache these results for later.
            _authorizationPermissionsCache.SetEntry(new AuthorizationPermissionsCacheEntry(
                guildId:                guildId,
                userId:                 userId,
                grantedPermissionIds:   grantedPermissionIds));

            return grantedPermissionIds;
        }

        private readonly IOptions<AuthorizationConfiguration>   _authorizationConfiguration;
        private readonly IAuthorizationPermissionsCache         _authorizationPermissionsCache;
        private readonly IDiscordRestGuildAPI                   _discordRestGuildAPI;
        private readonly IPermissionsRepository                 _permissionsRepository;

        private IReadOnlyCollection<int>? _allPermissions;
    }
}
