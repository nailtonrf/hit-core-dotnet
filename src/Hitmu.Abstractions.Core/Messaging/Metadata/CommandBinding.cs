using System;
using System.Reflection;

namespace Hitmu.Abstractions.Core.Messaging.Metadata
{
    public sealed class CommandBinding : Binding
    {
        public CommandBinding(Type handlerType, Type messageRequestType, Type messageResponseType)
            : base(BindingType.Command, messageRequestType)
        {
            MessageResponseType = messageResponseType;
            HandlerType = handlerType;
            HandlerMethod = HandlerType.GetMethod(MessagingConstants.CommandHandlerMethodName,
                new[] {messageRequestType, MessagingConstants.CancelationTokenType});
        }

        public Type MessageResponseType { get; }
        public override Type HandlerType { get; }
        public override MethodInfo HandlerMethod { get; }
    }
}