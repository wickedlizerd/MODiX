using System.Text.RegularExpressions;

using Microsoft.AspNetCore.Authorization;

namespace Modix.Web.Server.Authorization
{
    public class PermissionRequirement
        : IAuthorizationRequirement
    {
        public static PermissionRequirement Create(object permission)
            => new((int)permission, permission.GetType().Name, permission.ToString()!);

        public static PermissionRequirement Parse(string value)
        {
            var match = _parseRegex.Match(value);

            return new(
                permissionId:       int.Parse(match.Groups["id"].Value),
                permissionCategory: match.Groups["category"].Value,
                permissionName:     match.Groups["name"].Value);
        }

        private PermissionRequirement(
            int     permissionId,
            string  permissionCategory,
            string  permissionName)
        {
            PermissionId        = permissionId;
            PermissionCategory  = permissionCategory;
            PermissionName      = permissionName;
        }

        public int PermissionId { get; }

        public string PermissionCategory { get; }

        public string PermissionName { get; }

        public override string ToString()
            => $"{PermissionCategory}.{PermissionName}({PermissionId})";

        private static readonly Regex _parseRegex
            = new(@"^(?<category>\w+)\.(?<name>\w+)\((?<id>)\)$");
    }
}
