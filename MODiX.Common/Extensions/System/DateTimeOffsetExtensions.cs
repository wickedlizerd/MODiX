namespace System
{
    public static class DateTimeOffsetExtensions
    {
        public static DateTimeOffset TruncateMilliseconds(this DateTimeOffset value)
            => value.AddTicks(-(value.Ticks % TimeSpan.TicksPerSecond));
    }
}
