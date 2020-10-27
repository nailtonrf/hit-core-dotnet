using System;
using System.Reflection;

namespace Hitmu.Abstractions.Core.Messaging.Metadata
{
    public sealed class QueryBinding : Binding
    {
        public QueryBinding(Type handlerType, Type messageRequestType, Type messageResponseType)
            : base(BindingType.Query, messageRequestType)
        {
            MessageResponseType = messageResponseType;
            HandlerType = handlerType;
            HandlerMethod = HandlerType.GetMethod(MessagingConstants.QueryHandlerMethodName,
                new[] {messageRequestType, MessagingConstants.CancelationTokenType});
        }

        public Type MessageResponseType { get; }
        public override Type HandlerType { get; }
        public override MethodInfo HandlerMethod { get; }
    }
}