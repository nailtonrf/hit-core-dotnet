using System;
using System.Collections.Generic;

namespace Hitmu.Abstractions.Core.Results
{
    public struct Result<TValue> : IEquatable<Result<TValue>>
    {
        public bool IsValid { get; }
        public TValue Value { get; }
        public ErrorMessage ErrorMessage { get; }

        internal Result(TValue value) : this()
        {
            Value = value;
            IsValid = true;
        }

        internal Result(ErrorMessage errorMessage) : this()
        {
            ErrorMessage = errorMessage;
            IsValid = false;
        }

        public override string ToString()
        {
            return IsValid
                ? $"Valid<{typeof(TValue).Name}>({Value})"
                : $"ErrorResult<{typeof(TValue).Name}>({ErrorMessage})";
        }

        public override bool Equals(object obj)
        {
            return obj is Result<TValue> result && Equals(result);
        }

        internal bool IsInvalid => !IsValid;

        public bool Equals(Result<TValue> other)
        {
            return IsInvalid && other.IsInvalid &&
                   EqualityComparer<ErrorMessage>.Default.Equals(ErrorMessage, other.ErrorMessage)
                   || IsValid && other.IsValid && EqualityComparer<TValue>.Default.Equals(Value, other.Value);
        }

        public override int GetHashCode()
        {
            return IsValid
                ? Value == null ? base.GetHashCode() ^ 29 : Value.GetHashCode() ^ 31
                : ErrorMessage.GetHashCode() ^ 13;
        }

        public static bool operator ==(in Result<TValue> a, in Result<TValue> b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(in Result<TValue> a, in Result<TValue> b)
        {
            return !a.Equals(b);
        }

        public static implicit operator Result<TValue>(ErrorMessage errorMessage)
        {
            return Error<TValue>(errorMessage);
        }

        public static Result<T> From<T>(T value)
        {
            return new Result<T>(value);
        }

        public static Result<T> Error<T>(ErrorMessage errorMessage)
        {
            return new Result<T>(errorMessage);
        }
    }
}
