using System;
using System.Collections.Generic;

namespace Modix.Common.ObjectModel
{
    public static class Optional
    {
        public static Optional<T> FromValue<T>(T value)
            => Optional<T>.FromValue(value);

        public static Optional<T> Unspecified<T>()
            => Optional<T>.Unspecified;
    }

    // TODO: Get rid of this, I think. I honestly can't remember why I wrote this, but there was a specific reason, and I think it was related to how Remora's Optional<T> mishandled nulls. That's been fixed now, so this should be okay to drop.
    public readonly struct Optional<T>
        : IEquatable<Optional<T>>
    {
        public static Optional<T> FromValue(T value)
            => new(true, value);
        public static readonly Optional<T> Unspecified
            = default;

        private Optional(bool isSpecified, T value)
        {
            _isSpecified    = isSpecified;
            _value          = value;
        }

        public bool IsSpecified
            => _isSpecified;

        public T Value
            => _isSpecified
                ? _value
                : throw new InvalidOperationException("An attempt was made to access an unspecified optional value.");

        public bool Equals(Optional<T> other)
            => (_isSpecified == other.IsSpecified)
                && EqualityComparer<T>.Default.Equals(_value, other._value);

        public override bool Equals(object? obj)
            => (obj is Optional<T> other)
                && Equals(other);

        public override int GetHashCode()
            => HashCode.Combine(_isSpecified, _value);

        public static implicit operator Optional<T>(T value)
            => FromValue(value);

        public static implicit operator Optional<T>(Remora.Discord.Core.Optional<T> optional)
            => optional.HasValue
                ? FromValue(optional.Value)
                : Unspecified;

        public static bool operator ==(Optional<T> x, Optional<T> y)
            => x.Equals(y);

        public static bool operator !=(Optional<T> x, Optional<T> y)
            => !x.Equals(y);

        public override string ToString()
            => _isSpecified
                ? $"{{{_value?.ToString() ?? "null"}}}"
                : nameof(Unspecified);

        private readonly bool   _isSpecified;
        private readonly T      _value;
    }
}
