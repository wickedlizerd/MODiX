using System;

namespace System
{
    public static class DateTimeExtensions
    {
        public static DateTime TruncateMilliseconds(this DateTime value)
            => value.AddMilliseconds(-value.Millisecond);
    }
}
