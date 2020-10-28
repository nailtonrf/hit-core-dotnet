using FluentAssertions;
using Hitmu.Abstractions.Core.Results;
using Xunit;

namespace Hitmu.Abstractions.Core.Tests
{
    public class ErrorResultTests
    {
        [Fact]
        public void Should_Express_Property_ToString_With_Code()
        {
            var error = ErrorMessage.Error("1", "message", "property");
            error.ToString().Should().Be("property-message");
        }

        [Fact]
        public void Should_Express_ToString()
        {
            var error = ErrorMessage.Error("message");
            error.ToString().Should().Be("message");
        }

        [Fact]
        public void Should_Express_ToString_With_Code()
        {
            var error = ErrorMessage.Error("1", "message");
            error.ToString().Should().Be("message");
        }
    }
}