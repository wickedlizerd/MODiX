using System.ComponentModel.DataAnnotations;

namespace Modix.Bot
{
    public class ModixBotConfiguration
    {
        [Required]
        public string BotToken { get; set; }
            = null!;
    }
}
