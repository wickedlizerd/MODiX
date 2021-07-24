using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

using Remora.Discord.Core;

using Modix.Web.Protocol.Authorization;

namespace Modix.Web.Server.Authorization
{
    public class PermissionRequirementHandler
        : AuthorizationHandler<PermissionRequirement>
    {
        public PermissionRequirementHandler(
            Business.Authorization.IAuthorizationService    authorizationService,
            IHttpContextAccessor                            httpContextAccessor,
            ILogger<PermissionRequirementHandler>           logger)
        {
            _authorizationService   = authorizationService;
            _httpContextAccessor    = httpContextAccessor;
            _logger                 = logger;
        }

        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            PermissionRequirement       requirement)
        {
            AuthorizationLogMessages.PermissionRequirementHandling(_logger, requirement);

            AuthorizationLogMessages.UserIdRetrieving(_logger);
            var userIdClaimValue = context.User.Claims
                .Where(c => c.Type == ClaimTypes.NameIdentifier)
                .Select(c => c.Value)
                .FirstOrDefault();
            if (userIdClaimValue is null)
            {
                AuthorizationLogMessages.UserIdNotFound(_logger);
                return;
            }
            if (!ulong.TryParse(userIdClaimValue, out var userIdValue))
            {
                AuthorizationLogMessages.UserIdInvalid(_logger, userIdClaimValue);
                return;
            }
            var userId = new Snowflake(userIdValue);
            AuthorizationLogMessages.UserIdRetrieved(_logger, userId);

            AuthorizationLogMessages.GuildIdRetrieving(_logger);
            if ((_httpContextAccessor.HttpContext is null)
                || !_httpContextAccessor.HttpContext.Request.Headers.TryGetValue(AuthorizationConstants.GuildIdHeaderName, out var guildIdHeader))
            {
                AuthorizationLogMessages.GuildIdNotFound(_logger);
                return;
            }
            if ((guildIdHeader.Count != 1)
                || !ulong.TryParse(guildIdHeader[0], out var guildIdValue))
            {
                AuthorizationLogMessages.GuildIdInvalid(_logger, guildIdHeader.ToString());
                return;
            }
            var guildId = new Snowflake(guildIdValue);
            AuthorizationLogMessages.GuildIdRetrieved(_logger, guildId);

            AuthorizationLogMessages.GrantedPermissionIdsRetrieving(_logger, userId, guildId);
            var grantedPermissionIdsResult = await _authorizationService.GetGrantedPermissionIdsAsync(userId, guildId, _httpContextAccessor.HttpContext.RequestAborted);
            if (!grantedPermissionIdsResult.IsSuccess)
            {
                AuthorizationLogMessages.GrantedPermissionIdsRetrievalFailed(_logger, userId, guildId, grantedPermissionIdsResult.Error);
                return;
            }
            AuthorizationLogMessages.GrantedPermissionIdsRetrieved(_logger, userId, guildId, grantedPermissionIdsResult.Entity.Count);

            if (grantedPermissionIdsResult.Entity.Contains(requirement.PermissionId))
            {
                AuthorizationLogMessages.PermissionRequirementSatisfied(_logger, requirement);
                context.Succeed(requirement);
            }
            else
                AuthorizationLogMessages.PermissionRequirementNotSatisfied(_logger, requirement);
        }

        private readonly Business.Authorization.IAuthorizationService   _authorizationService;
        private readonly IHttpContextAccessor                           _httpContextAccessor;
        private readonly ILogger                                        _logger;
    }
}
