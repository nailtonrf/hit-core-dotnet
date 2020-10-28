using System;
using System.Linq;

namespace Hitmu.Abstractions.Core.Messaging.Metadata
{
    internal static class BindingFactory
    {
        public static Binding Create(BindingType bindingType, Type handlerType, Type[] requestArguments)
        {
            switch (bindingType)
            {
                case BindingType.Command:
                    return new CommandBinding(handlerType, requestArguments.FirstOrDefault(),
                        requestArguments.LastOrDefault());
                case BindingType.IntegrationEvent:
                    return new IntegrationEventBinding(handlerType, requestArguments.FirstOrDefault());
                case BindingType.Query:
                    return new QueryBinding(handlerType, requestArguments.FirstOrDefault(),
                        requestArguments.LastOrDefault());
                case BindingType.Event:
                    return new EventBinding(handlerType, requestArguments.FirstOrDefault());
            }
            throw new InvalidOperationException($"BindigType {bindingType} does not exists.");
        }
    }
}