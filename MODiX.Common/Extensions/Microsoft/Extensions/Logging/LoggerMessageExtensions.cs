using System;

namespace Microsoft.Extensions.Logging
{
    public static class LoggerMessageExtensions
    {
        public static Action<ILogger> WithoutException(
                this Action<ILogger, Exception?> action)
            => logger => action.Invoke(logger, null);

        public static Action<ILogger, T1> WithoutException<T1>(
                this Action<ILogger, T1, Exception?> action)
            => (logger, value1) => action.Invoke(logger, value1, null);

        public static Action<ILogger, T1, T2> WithoutException<T1, T2>(
                this Action<ILogger, T1, T2, Exception?> action)
            => (logger, value1, value2) => action.Invoke(logger, value1, value2, null);

        public static Action<ILogger, T1, T2, T3> WithoutException<T1, T2, T3>(
                this Action<ILogger, T1, T2, T3, Exception?> action)
            => (logger, value1, value2, value3) => action.Invoke(logger, value1, value2, value3, null);

        public static Action<ILogger, T1, T2, T3, T4> WithoutException<T1, T2, T3, T4>(
                this Action<ILogger, T1, T2, T3, T4, Exception?> action)
            => (logger, value1, value2, value3, value4) => action.Invoke(logger, value1, value2, value3, value4, null);

        public static Action<ILogger, T1, T2, T3, T4, T5> WithoutException<T1, T2, T3, T4, T5>(
                this Action<ILogger, T1, T2, T3, T4, T5, Exception?> action)
            => (logger, value1, value2, value3, value4, value5) => action.Invoke(logger, value1, value2, value3, value4, value5, null);

        public static Action<ILogger, T1, T2, T3, T4, T5, T6> WithoutException<T1, T2, T3, T4, T5, T6>(
                this Action<ILogger, T1, T2, T3, T4, T5, T6, Exception?> action)
            => (logger, value1, value2, value3, value4, value5, value6) => action.Invoke(logger, value1, value2, value3, value4, value5, value6, null);
    }
}
