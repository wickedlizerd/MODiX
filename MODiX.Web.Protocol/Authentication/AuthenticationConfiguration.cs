using System;
using System.ComponentModel.DataAnnotations;

namespace Modix.Web.Protocol.Authentication
{
    public class AuthenticationConfiguration
    {
        [Required]
        public TimeSpan TokenLifetime { get; init; }

        [Required]
        public string TokenSignatureSecret { get; init; }
            = null!;
    }
}
