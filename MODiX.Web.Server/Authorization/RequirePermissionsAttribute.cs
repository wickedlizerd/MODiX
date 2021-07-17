using System.Linq;

using Microsoft.AspNetCore.Authorization;

namespace Modix.Web.Server.Authorization
{
    public class RequirePermissionsAttribute
        : AuthorizeAttribute
    {
        public RequirePermissionsAttribute(params object[] permissions)
            : base(new PermissionRequirementPolicy(permissions
                    .Select(permission => PermissionRequirement.Create(permission))
                    .ToArray())
                .ToName()) { }
    }
}
