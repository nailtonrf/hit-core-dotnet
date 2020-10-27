using System;
using System.Linq;

namespace Hitmu.Abstractions.Core.Messaging.Metadata
{
    internal static class BindingFactory
    {
        public static Binding Create(BindingType bindingType, Type handlerType, Type[] requestArguments)
        {
            return bindingType switch
            {
                BindingType.Command => new CommandBinding(handlerType, requestArguments.FirstOrDefault(),
                    requestArguments.LastOrDefault()),
                BindingType.IntegrationEvent => new IntegrationEventBinding(handlerType,
                    requestArguments.FirstOrDefault()),
                BindingType.Query => new QueryBinding(handlerType, requestArguments.FirstOrDefault(),
                    requestArguments.LastOrDefault()),
                BindingType.Event => new EventBinding(handlerType, requestArguments.FirstOrDefault()),
                _ => throw new InvalidOperationException($"BindigType {bindingType} does not exists.")
            };
        }
    }
}