using System;
using System.Reflection;

namespace Hitmu.Abstractions.Core.Messaging.Metadata
{
    public sealed class EventBinding : Binding
    {
        public EventBinding(Type handlerType, Type messageRequestType) : base(BindingType.Event, messageRequestType)
        {
            HandlerType = handlerType;
            HandlerMethod = HandlerType.GetMethod(MessagingConstants.EventHandlerName,
                new[] {messageRequestType, MessagingConstants.CancelationTokenType});
        }

        public override Type HandlerType { get; }
        public override MethodInfo HandlerMethod { get; }
    }
}