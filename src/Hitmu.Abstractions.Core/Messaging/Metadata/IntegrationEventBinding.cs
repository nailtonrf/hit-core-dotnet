using System;
using System.Reflection;

namespace Hitmu.Abstractions.Core.Messaging.Metadata
{
    public sealed class IntegrationEventBinding : Binding
    {
        public IntegrationEventBinding(Type handlerType, Type messageRequestType) : base(BindingType.IntegrationEvent,
            messageRequestType)
        {
            HandlerType = handlerType;
            HandlerMethod = HandlerType.GetMethod(MessagingConstants.IntegrationEventHandlerName, new[] { messageRequestType });
        }

        public override Type HandlerType { get; }
        public override MethodInfo HandlerMethod { get; }
    }
}