using System;

using Remora.Results;

namespace Modix.Bot.Controls
{
    public class ControlException
        : Exception
    {
        public static ControlException FromError(string message, IResultError error)
            => new ControlException($"{message}: {error.Message}", (error as ExceptionError)?.Exception);

        public ControlException() { }

        public ControlException(string? message)
            : base(message)
        { }

        public ControlException(string? message, Exception? inner)
            : base(message, inner)
        { }
    }
}
