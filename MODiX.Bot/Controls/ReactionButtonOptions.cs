using Remora.Results;

namespace Modix.Bot.Controls
{
    public class ReactionButtonOptions
    {
        public ReactionButtonOptions(
            string emoji,
            OperationAction onClickedAsync)
        {
            Emoji = emoji;
            OnClickedAsync = onClickedAsync;
        }

        public string Emoji { get; }

        public OperationAction OnClickedAsync { get; }
    }
}
