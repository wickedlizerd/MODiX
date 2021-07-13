using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

using Remora.Discord.Core;

using Modix.Web.Protocol.Authorization;

using IAuthorizationService = Modix.Business.Authorization.IAuthorizationService;

namespace Modix.Web.Server.Authorization
{
    public class PermissionRequirementHandler
        : AuthorizationHandler<PermissionRequirement>
    {
        public PermissionRequirementHandler(
            IAuthorizationService   authorizationService,
            IHttpContextAccessor    httpContextAccessor)
        {
            _authorizationService   = authorizationService;
            _httpContextAccessor    = httpContextAccessor;
        }

        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            PermissionRequirement       requirement)
        {
            var userId = context.User.Claims
                .Where(c => c.Type == ClaimTypes.NameIdentifier)
                .Select(c => new Snowflake(ulong.Parse(c.Value)) as Snowflake?)
                .FirstOrDefault();
            if (userId is null)
                return;

            if ((_httpContextAccessor.HttpContext is null)
                    || !_httpContextAccessor.HttpContext.Request.Headers.TryGetValue(AuthorizationConstants.GuildIdHeaderName, out var guildIdHeader)
                    || (guildIdHeader.Count != 1)
                    || !ulong.TryParse(guildIdHeader[0], out var guildIdValue))
                return;

            var guildId = new Snowflake(guildIdValue);

            var grantedPermissionIds = await _authorizationService.GetGrantedPermissionIdsAsync(guildId, userId.Value, _httpContextAccessor.HttpContext.RequestAborted);
            if (!grantedPermissionIds.IsSuccess)
                return;

            if (grantedPermissionIds.Entity.Contains(requirement.PermissionId))
                context.Succeed(requirement);
        }

        private readonly IAuthorizationService  _authorizationService;
        private readonly IHttpContextAccessor   _httpContextAccessor;
    }
}
