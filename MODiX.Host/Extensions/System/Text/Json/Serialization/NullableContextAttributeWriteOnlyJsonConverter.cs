namespace System.Text.Json.Serialization
{
    // https://github.com/dotnet/runtime/issues/58690
    public class NullableContextAttributeWriteOnlyJsonConverter
        : JsonConverter<object>
    {
        public override bool CanConvert(Type typeToConvert)
            => typeToConvert.FullName is "System.Runtime.CompilerServices.NullableContextAttribute";

        public override object? Read(
                ref Utf8JsonReader      reader,
                Type                    typeToConvert,
                JsonSerializerOptions   options)
            => throw new NotSupportedException();

        public override void Write(
                Utf8JsonWriter          writer,
                object                  value,
                JsonSerializerOptions   options)
            => writer.WriteStringValue("System.Runtime.CompilerServices.NullableContextAttribute");
    }
}
