using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Modix.Business.Authorization
{
    public class AuthorizationConfiguration
    {
        [Required]
        public IReadOnlyList<ulong> AdminUserIds { get; init; }
            = null!;
    }
}
