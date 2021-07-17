using System;
using System.ComponentModel.DataAnnotations;

namespace Modix.Host.Logging
{
    public class SeqConfiguration
    {
        [Required]
        public string ApiKey { get; init; }
            = null!;

        [Required]
        public string ServerUrl { get; init; }
            = null!;
    }
}
