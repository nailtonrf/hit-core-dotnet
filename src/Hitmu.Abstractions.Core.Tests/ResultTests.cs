using FluentAssertions;
using Hitmu.Abstractions.Core.Results;
using Xunit;

namespace Hitmu.Abstractions.Core.Tests
{
    public class ResultTests
    {
        [Fact]
        public void Should_Create_Error_Result()
        {
            var result = Result<int>.Error<int>(ErrorMessage.Error("error"));
            result.IsValid.Should().BeFalse();
            result.ErrorMessage.ToString().Should().Be("error");
        }

        [Fact]
        public void Should_Create_Error_Result_Equals()
        {
            var result = Result<int>.Error<int>(ErrorMessage.Error("error"));
            var result2 = Result<int>.Error<int>(ErrorMessage.Error("error"));
            result.Equals(result2).Should().BeTrue();
        }

        [Fact]
        public void Should_Create_Error_Result_ToString()
        {
            var result = Result<int>.Error<int>(ErrorMessage.Error("error"));
            result.ToString().Should().Be("ErrorResult<Int32>(error)");
        }

        [Fact]
        public void Should_Create_Ok_Result()
        {
            var result = Result<int>.From(10);
            result.IsValid.Should().BeTrue();
            result.Value.Should().Be(10);
        }

        [Fact]
        public void Should_Create_Ok_Result_Equals()
        {
            var result = Result<int>.From(10);
            var result2 = Result<int>.From(10);
            result.Equals(result2).Should().BeTrue();
        }

        [Fact]
        public void Should_Create_Ok_Result_ToString()
        {
            var result = Result<int>.From(10);
            result.ToString().Should().Be("Valid<Int32>(10)");
        }
    }
}