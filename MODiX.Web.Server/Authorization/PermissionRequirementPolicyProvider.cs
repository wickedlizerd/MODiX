using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace Modix.Web.Server.Authorization
{
    public class PermissionRequirementPolicyProvider
        : DefaultAuthorizationPolicyProvider
    {
        public PermissionRequirementPolicyProvider(IOptions<AuthorizationOptions> options)
            : base(options) { }

        public override Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
            => policyName.StartsWith(PermissionRequirementPolicy.NamePrefix)
                ? Task.FromResult<AuthorizationPolicy?>(
                    new PermissionRequirementPolicy(int.Parse(policyName[PermissionRequirementPolicy.NamePrefix.Length..])))
                : base.GetPolicyAsync(policyName);
    }
}
