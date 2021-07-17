using System;
using System.Linq;

using Microsoft.AspNetCore.Authorization;

namespace Modix.Web.Server.Authorization
{
    public class PermissionRequirementPolicy
        : AuthorizationPolicy
    {
        public static PermissionRequirementPolicy? TryParseName(string name)
        {
            var pieces = name.Split(NameSeparator);
            return ((pieces.Length >= 1) && (pieces[0] == NamePrefix))
                ? new(pieces
                    .Skip(1)
                    .Select(piece => PermissionRequirement.Parse(piece))
                    .ToArray())
                : null;
        }

        public PermissionRequirementPolicy(params PermissionRequirement[] requirements)
            : base(
                requirements,
                Array.Empty<string>()) { }

        public string ToName()
            => string.Join(
                NameSeparator,
                Requirements
                    .Select(requirement => requirement.ToString())
                    .Prepend(NamePrefix));

        private const string    NamePrefix      = "RequirePermissions";
        private const char      NameSeparator   = ':';
    }
}
