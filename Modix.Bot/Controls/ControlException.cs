using System;

namespace Modix.Bot.Controls
{
    public class ControlException
        : Exception
    {
        public ControlException() { }

        public ControlException(string? message)
            : base(message)
        { }

        public ControlException(string? message, Exception? inner)
            : base(message, inner)
        { }
    }
}
