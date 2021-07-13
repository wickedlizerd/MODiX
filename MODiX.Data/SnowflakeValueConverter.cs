using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

using Remora.Discord.Core;

namespace Modix.Data
{
    internal class SnowflakeValueConverter
        : ValueConverter<Snowflake, long>
    {
        public static readonly SnowflakeValueConverter Default
            = new();

        private SnowflakeValueConverter()
            : base(
                x => (long)x.Value,
                x => new Snowflake((ulong)x)) { }
    }
}
