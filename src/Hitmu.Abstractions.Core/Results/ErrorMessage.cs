using Hitmu.Abstractions.Core.Resilience;
using System;

namespace Hitmu.Abstractions.Core.Results
{
    public struct ErrorMessage
    {
        public string Message { get; }
        public string? Code { get; }
        public string? PropertyName { get; }
        public ResilienceErrorType ErrorType { get; }

        public ErrorMessage(string message, string? code, string? propertyName, ResilienceErrorType errorType = ResilienceErrorType.Transient)
        {
            Message = message ?? throw new ArgumentNullException(nameof(message));
            Code = code;
            PropertyName = propertyName;
            ErrorType = errorType;
        }

        public ErrorMessage(string message, string code) : this(message, code, null)
        {
        }

        public ErrorMessage(string message) : this(message, code: null, propertyName: null)
        {
        }

        public ErrorMessage(string message, ResilienceErrorType errorType) : this(message, null, null, errorType)
        {
        }

        public ErrorMessage(string message, string code, ResilienceErrorType errorType) : this(message, code, null, errorType)
        {
        }

        public override string ToString() => string.IsNullOrWhiteSpace(PropertyName) ? $"{Message}" : $"{PropertyName}-{Message}";

        public static ErrorMessage Error(string message) => new ErrorMessage(message);

        public static ErrorMessage Error(string code, string message) => new ErrorMessage(message, code);

        public static ErrorMessage Error(string code, string message, string propertyName) => new ErrorMessage(message, code, propertyName);

        public static ErrorMessage Error(string message, ResilienceErrorType errorType) => new ErrorMessage(message, errorType);

        public static ErrorMessage Error(string code, string message, ResilienceErrorType errorType) => new ErrorMessage(message, code, errorType);
    }
}