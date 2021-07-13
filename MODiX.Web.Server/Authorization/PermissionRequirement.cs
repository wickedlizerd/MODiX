using Microsoft.AspNetCore.Authorization;

namespace Modix.Web.Server.Authorization
{
    public class PermissionRequirement
        : IAuthorizationRequirement
    {
        public PermissionRequirement(int permissionId)
            => PermissionId = permissionId;

        public int PermissionId { get; }
    }
}
