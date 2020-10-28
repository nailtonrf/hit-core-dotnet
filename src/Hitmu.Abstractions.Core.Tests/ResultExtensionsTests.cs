using FluentAssertions;
using Hitmu.Abstractions.Core.Results;
using System;
using System.Diagnostics;
using Xunit;

namespace Hitmu.Abstractions.Core.Tests
{
    public class ResultExtensionsTests
    {
        [Fact]
        public void Should_Bind_Error_Result()
        {
            var errorResult = Result<int>.From(10)
                .Then(r1 => Result<int>.From(r1 / 2))
                .Then(r2 => Result<int>.Error<int>(ErrorMessage.Error("error")))
                .Then(r3 => Result<int>.From(r3 * 2));
            errorResult.IsValid.Should().BeFalse();
            errorResult.ErrorMessage.Message.Should().Be("error");
        }

        [Fact]
        public void Should_Bind_Ok_Result()
        {
            Result<int>.From(10)
                .Then(r1 => Result<int>.From(r1 / 2))
                .Then(r2 => Result<int>.From(r2 / 5))
                .GetOrDefault()
                .Should().Be(1);
        }

        [Fact]
        public void Should_Do_Dead_End_Action()
        {
            Result<int>.From(10)
                .Do(x => Debug.WriteLine(x))
                .Value.Should().Be(10);
        }

        [Fact]
        public void Should_DoWhenError_Dead_End_Action()
        {
            var whenOk = 0;
            var whenError = 0;
            var result = Result<int>.Error<int>(ErrorMessage.Error("error"))
                .Do(_ => whenOk++)
                .DoWhenError(_ => whenError++);
            result.IsValid.Should().BeFalse();
            whenOk.Should().Be(0);
            whenError.Should().Be(1);
        }

        [Fact]
        public void Should_Map_Result()
        {
            Result<double>.From(1000.00)
                .Then(r1 => Result<int>.From(r1 - 500.00))
                .Map(r2 => new AccountPosition(DateTime.UtcNow, r2))
                .Value.Position.Should().Be(500.00);
        }

        [Fact]
        public void Should_Ok_GetDefaultValue()
        {
            var resultOk = Result<int>.From(1);
            resultOk.GetOrDefault().Should().Be(1);
        }

        [Fact]
        public void Should_Ok_GetDefaultValue_Default()
        {
            var resultOk = Result<int>.Error<int>(ErrorMessage.Error("error"));
            resultOk.GetOrDefault().Should().Be(0);
        }

        [Fact]
        public void Should_TryCatch_SuccessResult()
        {
            static int Fun()
            {
                return 10;
            }

            Result<double>.From(1000.00)
                .Then(r1 => Result<int>.From(r1 - 500.00))
                .Then(r2 => Result<double>.From(r2 / Fun()))
                .IsValid.Should().BeTrue();
        }

        [Fact]
        public void Should_Convert_ErrorResult_To_ResultFail()
        {
            Result<int> result = ErrorMessage.Error("error");
            result.IsValid.Should().BeFalse();
        }

        [Fact]
        public void Should_Result_Be_Equals()
        {
            var result1 = Result<int>.From(10);
            var result2 = Result<int>.From(10);
            result1.Should().Be(result2);
        }

        [Fact]
        public void Should_Result_Be_NotEquals()
        {
            var result1 = Result<int>.From(10);
            var result2 = Result<int>.From(20);
            result1.Should().NotBe(result2);
        }
    }

    #region Setup

    public class AccountPosition
    {
        public AccountPosition(DateTime when, double position)
        {
            When = when;
            Position = position;
        }

        public DateTime When { get; }
        public double Position { get; }
    }

    #endregion
}