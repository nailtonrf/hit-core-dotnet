using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace Hitmu.Abstractions.Interactions
{
    public static class AssemblyScanner
    {
        public static void InitializeDecorator(this IServiceCollection services, Type decoratedType, Type decorator)
        {
            if (services.Any(p => p.ServiceType.IsGenericType && p.ServiceType.GetGenericTypeDefinition() == decoratedType))
            {
                services.Decorate(decoratedType, decorator);
            }
        }
    }
}