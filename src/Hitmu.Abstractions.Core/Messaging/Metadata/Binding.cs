using System;
using System.Reflection;

namespace Hitmu.Abstractions.Core.Messaging.Metadata
{
    public abstract class Binding
    {
        protected Binding(BindingType bindingType, Type messageRequestType)
        {
            Type = bindingType;
            MessageRequestType = messageRequestType;
            MessageTypeName = messageRequestType.Name.ToLower();
        }

        public string MessageTypeName { get; }
        public BindingType Type { get; }
        public abstract Type HandlerType { get; }
        public Type MessageRequestType { get; }
        public abstract MethodInfo HandlerMethod { get; }
    }
}