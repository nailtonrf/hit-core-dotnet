using Hitmu.Abstractions.Context;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Hitmu.Abstractions.Interactions.IntegrationTests.Helpers
{
    internal static class ApplicationContextExtensions
    {
        public static IApplicationContext CreateApplicationContext()
        {
            return CreateApplicationContext(new ServiceCollection());
        }

        public static IApplicationContext CreateApplicationContext(ServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<ILoggerFactory, NullLoggerFactory>();
            var applicationContext = ApplicationContext.CreateContext("app", serviceCollection);
            return applicationContext
                .Load(new ApplicationInitializer())
                .Load(new DefaultRequestScopeInitializer());
        }
    }
}