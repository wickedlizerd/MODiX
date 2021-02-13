using Remora.Results;

namespace Modix.Bot.Extensions.Remora.Results
{
    public record ParsingError<T>
        : ResultError
    {
        public ParsingError(string value)
                : base($"Unable to parse value of type {typeof(T).Name}")
            => Value = value;

        public string Value { get; }
    }
}
