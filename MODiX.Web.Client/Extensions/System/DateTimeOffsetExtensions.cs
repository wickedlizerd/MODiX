using System;

namespace System
{
    public static class DateTimeOffsetExtensions
    {
        public static DateTimeOffset TruncateMilliseconds(this DateTimeOffset value)
            => value.AddMilliseconds(-value.Millisecond);
    }
}
