using System;

namespace Hitmu.Abstractions.Core.Results
{
    public struct ErrorMessage
    {
        public string Message { get; }
        public string? Code { get; }
        public string? PropertyName { get; }

        public ErrorMessage(string message, string? code, string? propertyName)
        {
            Message = message ?? throw new ArgumentNullException(nameof(message));
            Code = code;
            PropertyName = propertyName;
        }

        public ErrorMessage(string message, string code) : this(message, code, null) { }

        public ErrorMessage(string message) : this(message, code: null, propertyName: null) { }

        public override string ToString()
        {
            return string.IsNullOrWhiteSpace(PropertyName) ? $"{Message}" : $"{PropertyName}-{Message}";
        }

        public static ErrorMessage Error(string message)
        {
            return new ErrorMessage(message);
        }

        public static ErrorMessage Error(string code, string message)
        {
            return new ErrorMessage(code, message);
        }
    }
}