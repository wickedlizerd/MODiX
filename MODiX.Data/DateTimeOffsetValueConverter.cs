using System;

using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Modix.Data
{
    internal class DateTimeOffsetValueConverter
        : ValueConverter<DateTimeOffset, DateTime>
    {
        public static readonly DateTimeOffsetValueConverter Default
            = new();

        private DateTimeOffsetValueConverter()
            : base(
                x => x.UtcDateTime,
                x => new DateTimeOffset(x.ToUniversalTime())) { }
    }
}
