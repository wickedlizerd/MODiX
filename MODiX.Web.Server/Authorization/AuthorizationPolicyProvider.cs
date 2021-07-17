using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace Modix.Web.Server.Authorization
{
    public class AuthorizationPolicyProvider
        : DefaultAuthorizationPolicyProvider
    {
        public AuthorizationPolicyProvider(IOptions<AuthorizationOptions> options)
            : base(options) { }

        public override async Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
            => PermissionRequirementPolicy.TryParseName(policyName)
                ?? await base.GetPolicyAsync(policyName);
    }
}
