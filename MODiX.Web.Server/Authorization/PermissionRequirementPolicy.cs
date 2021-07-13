using System;

using Microsoft.AspNetCore.Authorization;

namespace Modix.Web.Server.Authorization
{
    public class PermissionRequirementPolicy
        : AuthorizationPolicy
    {
        public const string NamePrefix
            = "RequirePermission:";

        public PermissionRequirementPolicy(int permissionId)
            : base(
                new[] { new PermissionRequirement(permissionId) },
                Array.Empty<string>()) { }
    }
}
