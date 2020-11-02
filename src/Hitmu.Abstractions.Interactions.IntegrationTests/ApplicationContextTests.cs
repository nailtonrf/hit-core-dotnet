using System;
using FluentAssertions;
using Hitmu.Abstractions.Context;
using Hitmu.Abstractions.Interactions.IntegrationTests.Helpers;
using Xunit;

namespace Hitmu.Abstractions.Interactions.IntegrationTests
{
    public class ApplicationContextTests
    {
        [Fact]
        public void Should_Create_ApplicationContext()
        {
            var applicationContext = ApplicationContextExtensions.CreateApplicationContext();
            applicationContext.ApplicationName.Should().Be("app");
            applicationContext.ApplicationId.Should().NotBeEmpty();
        }

        [Fact]
        public void Should_Create_ApplicationContext_Instance()
        {
            ApplicationContext.Clear();
            Func<ApplicationContext> func = () => ApplicationContext.Instance;
            func.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void ApplicationScope_NotBeNull()
        {
            var applicationContext = ApplicationContextExtensions.CreateApplicationContext();
            applicationContext.InitializeMediators();
            applicationContext.Start();
            using var scope = applicationContext.BeginScope();
            var applicationScope = scope.GetService<IRequestScope>();
            applicationScope.Should().NotBeNull();
        }
    }

    #region Setups

    public interface IAbstraction
    {
        string RequestContextId { get; }
    }

    public sealed class Implementation : IAbstraction
    {
        private readonly IIteratorContext _requestContext;

        public Implementation(IRequestScope applicationScope)
        {
            _requestContext = applicationScope.GetService<IIteratorContext>();
        }

        public string RequestContextId => _requestContext.Id.ToString();
    }

    #endregion
}