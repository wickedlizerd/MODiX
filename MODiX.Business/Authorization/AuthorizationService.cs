using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;
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
            ILogger<AuthorizationService>           logger,
            IPermissionsRepository                  permissionsRepository)
        {
            _authorizationConfiguration     = authorizationConfiguration;
            _authorizationPermissionsCache  = authorizationPermissionsCache;
            _discordRestGuildAPI            = discordRestGuildAPI;
            _logger                         = logger;
            _permissionsRepository          = permissionsRepository;
        }

        public async ValueTask<Result<IReadOnlyCollection<int>>> GetGrantedPermissionIdsAsync(
            Snowflake           guildId,
            Snowflake           userId,
            CancellationToken   cancellationToken)
        {
            AuthorizationLogMessages.GrantedPermissionIdsRetrieving(_logger, guildId, userId);

            using var @lock = await _authorizationPermissionsCache.LockAsync(cancellationToken);

            // If the user is a hard-coded admin, they get all permissions, period.
            if (_authorizationConfiguration.Value.AdminUserIds.Contains(userId.Value))
            {
                AuthorizationLogMessages.AdministratorIdentified(_logger, userId);

                if (_allPermissions is null)
                {
                    AuthorizationLogMessages.AllPermissionsRetrieving(_logger);
                    _allPermissions = await _permissionsRepository.AsyncEnumeratePermissionIds()
                        .ToHashSetAsync(cancellationToken);
                    AuthorizationLogMessages.AllPermissionsRetrieved(_logger, _allPermissions.Count);
                }

                return Result<IReadOnlyCollection<int>>.FromSuccess(_allPermissions);
            }

            // If the permissions for this users are cached, return those.
            var cacheEntry = _authorizationPermissionsCache.TryGetEntry((guildId, userId));
            if (cacheEntry is not null)
            {
                AuthorizationLogMessages.GrantedPermissionIdsRetrieved(_logger, guildId, userId, cacheEntry.GrantedPermissionIds.Count);
                return Result<IReadOnlyCollection<int>>.FromSuccess(cacheEntry.GrantedPermissionIds);
            }
            AuthorizationLogMessages.GrantedPermissionIdsNotFound(_logger, guildId, userId);

            AuthorizationLogMessages.GuildMemberRetrieving(_logger, guildId, userId);
            var guildMemberResult = await _discordRestGuildAPI.GetGuildMemberAsync(guildId, userId, cancellationToken);
            if (!guildMemberResult.IsSuccess)
            {
                var error = guildMemberResult.Unwrap();
                AuthorizationLogMessages.GuildMemberRetrievalFailed(_logger, error);
                return Result<IReadOnlyCollection<int>>.FromError(error);
            }

            var guildMember = guildMemberResult.Entity;
            if (!guildMember.Permissions.HasValue)
            {
                var error = new DataNotFoundError($"Guild {guildId} permissions for user {userId}");
                AuthorizationLogMessages.GuildMemberRetrievalFailed(_logger, error);
                return Result<IReadOnlyCollection<int>>.FromError(error);
            }
            AuthorizationLogMessages.GuildMemberRetrieved(_logger, guildId, userId, guildMember.Permissions.Value, guildMember.Roles.Count);

            var grantedPermissionIds = new HashSet<int>();

            // Map out permissions from the user's Guild Permissions
            AuthorizationLogMessages.GuildPermissionMappingsEnumerating(_logger, guildId);
            var guildPermissionMappings = _permissionsRepository.AsyncEnumerateGuildPermissionMappingDefinitions(
                guildId:    guildId,
                isDeleted:  false);
            await foreach(var mapping in guildPermissionMappings)
            {
                if (guildMember.Permissions.Value.HasPermission(mapping.GuildPermission))
                {
                    if (mapping.Type == PermissionMappingType.Grant)
                    {
                        AuthorizationLogMessages.GuildPermissionGranted(_logger, mapping);
                        grantedPermissionIds.Add(mapping.PermissionId);
                    }
                    else
                    {
                        AuthorizationLogMessages.GuildPermissionRevoked(_logger, mapping);
                        grantedPermissionIds.Remove(mapping.PermissionId);
                    }
                }
                else
                    AuthorizationLogMessages.GuildPermissionMappingIgnored(_logger, mapping);
            }
            AuthorizationLogMessages.GuildPermissionMappingsEnumerated(_logger, guildId);

            // Map out permissions from the user's Guild Roles, overwriting mappings from above, if applicable.
            AuthorizationLogMessages.RolePermissionMappingsEnumerating(_logger, guildId);
            var rolePermissionMappings = _permissionsRepository.AsyncEnumerateRolePermissionMappingDefinitions(
                guildId:    guildId,
                isDeleted:  false);
            await foreach (var mapping in rolePermissionMappings)
            {
                if (guildMember.Roles.Contains(mapping.RoleId))
                {
                    if (mapping.Type == PermissionMappingType.Grant)
                    {
                        AuthorizationLogMessages.RolePermissionGranted(_logger, mapping);
                        grantedPermissionIds.Add(mapping.PermissionId);
                    }
                    else
                    {
                        AuthorizationLogMessages.RolePermissionRevoked(_logger, mapping);
                        grantedPermissionIds.Remove(mapping.PermissionId);
                    }
                }
                else
                    AuthorizationLogMessages.RolePermissionMappingIgnored(_logger, mapping);
            }
            AuthorizationLogMessages.RolePermissionMappingsEnumerated(_logger, guildId);

            // Don't forget to cache these results for later.
            AuthorizationLogMessages.GrantedPermissionIdsCaching(_logger, guildId, userId, grantedPermissionIds.Count);
            _authorizationPermissionsCache.SetEntry(new AuthorizationPermissionsCacheEntry(
                guildId:                guildId,
                userId:                 userId,
                grantedPermissionIds:   grantedPermissionIds));
            AuthorizationLogMessages.GrantedPermissionIdsCached(_logger, guildId, userId, grantedPermissionIds.Count);

            return grantedPermissionIds;
        }

        private readonly IOptions<AuthorizationConfiguration>   _authorizationConfiguration;
        private readonly IAuthorizationPermissionsCache         _authorizationPermissionsCache;
        private readonly IDiscordRestGuildAPI                   _discordRestGuildAPI;
        private readonly ILogger                                _logger;
        private readonly IPermissionsRepository                 _permissionsRepository;

        private IReadOnlyCollection<int>? _allPermissions;
    }
}
