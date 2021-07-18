using System.ComponentModel.DataAnnotations;

namespace Modix.Data
{
    public class DataConfiguration
    {
        [Required]
        public string ConnectionString { get; init; }
            = null!;
    }
}
