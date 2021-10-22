using System;
using System.Text.Json;
using System.Text.Json.Serialization;

using Remora.Discord.Core;

namespace SeqLoggerProvider.Serialization
{
    public class SnowflakeWriteOnlyJsonConverter
        : JsonConverter<Snowflake>
    {
        public override Snowflake Read(
                ref Utf8JsonReader      reader,
                Type                    typeToConvert,
                JsonSerializerOptions   options)
            => throw new NotSupportedException();
        public override void Write(
                Utf8JsonWriter          writer,
                Snowflake               value,
                JsonSerializerOptions   options)
            => writer.WriteNumberValue(value.Value);
    }
}
