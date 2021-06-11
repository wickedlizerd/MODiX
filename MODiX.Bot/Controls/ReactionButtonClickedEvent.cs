using Remora.Discord.Core;

namespace Modix.Bot.Controls
{
    public class ReactionButtonClickedEvent
    {
        public ReactionButtonClickedEvent(
            string emojiName,
            Snowflake clickedBy)
        {
            EmojiName = emojiName;
            ClickedBy = clickedBy;
        }

        public string EmojiName { get; }

        public Snowflake ClickedBy { get; }
    }
}
