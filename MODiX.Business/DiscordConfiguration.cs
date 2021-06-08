using System.ComponentModel.DataAnnotations;

namespace Modix.Business
{
    public class DiscordConfiguration
    {
        [Required]
        public string BotToken { get; init; }
            = null!;

        [Required]
        public string ClientId { get; init; }
            = null!;

        [Required]
        public string ClientSecret { get; init; }
            = null!;
    }
}
