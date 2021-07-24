using Microsoft.Extensions.Logging;

namespace System.Transactions
{
    public static partial class TransactionsLogMessages
    {
        [LoggerMessage(
            EventId = 0x266260F5,
            Level   = LogLevel.Debug,
            Message = "Transaction scope committed")]
        public static partial void TransactionScopeCommitted(ILogger logger);

        [LoggerMessage(
            EventId = 0x388E8FA2,
            Level   = LogLevel.Debug,
            Message = "Committing transaction scope")]
        public static partial void TransactionScopeCommitting(ILogger logger);

        [LoggerMessage(
            EventId = 0x38435A05,
            Level   = LogLevel.Debug,
            Message = "Transaction scope created")]
        public static partial void TransactionScopeCreated(ILogger logger);

        [LoggerMessage(
            EventId = 0x6E89A5F8,
            Level   = LogLevel.Debug,
            Message = "Creating transaction scope")]
        public static partial void TransactionScopeCreating(ILogger logger);

        [LoggerMessage(
            EventId = 0x385B5CEA,
            Level   = LogLevel.Debug,
            Message = "Transaction scope disposed")]
        public static partial void TransactionScopeDisposed(ILogger logger);

        [LoggerMessage(
            EventId = 0x36FF63B6,
            Level   = LogLevel.Debug,
            Message = "Disposing transaction scope")]
        public static partial void TransactionScopeDisposing(ILogger logger);
    }
}
