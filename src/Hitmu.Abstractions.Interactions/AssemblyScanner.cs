using Hitmu.Abstractions.Interactions.Decorators;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection;

namespace Hitmu.Abstractions.Interactions
{
    internal static class AssemblyScanner
    {
        public static void InitializeDecorator(this IServiceCollection services, Type decoratedType, Type decorator)
        {
            if (services.Any(p => p.ServiceType.IsGenericType && p.ServiceType.GetGenericTypeDefinition() == decoratedType))
            {
                services.Decorate(decoratedType, decorator);
            }
        }

        public static void InitializeTypeWithTransientLifetime(this IServiceCollection services, Assembly source, Type type)
        {
            services.Scan(p =>
                p.FromAssemblies(source)
                    .AddClasses(c =>
                    {
                        c.AssignableTo(type);
                        c.NotInNamespaceOf(typeof(CommandHandlerDecorator<,>), typeof(EventHandlerDecorator<>));
                    })
                    .AsImplementedInterfaces()
                    .WithTransientLifetime());
        }

        public static void InitializeTypeWithSingletonLifetime(this IServiceCollection services, Assembly source, Type type)
        {
            services.Scan(p =>
                p.FromAssemblies(source)
                    .AddClasses(c =>
                    {
                        c.AssignableTo(type);
                        c.NotInNamespaceOf(typeof(CommandHandlerDecorator<,>), typeof(EventHandlerDecorator<>));
                    })
                    .AsImplementedInterfaces()
                    .WithSingletonLifetime());
        }
    }
}