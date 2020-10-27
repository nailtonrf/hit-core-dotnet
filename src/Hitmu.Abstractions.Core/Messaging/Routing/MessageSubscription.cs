using Hitmu.Abstractions.Core.Messaging.Metadata;

namespace Hitmu.Abstractions.Core.Messaging.Routing
{
    public sealed class MessageSubscription
    {
        public MessageSubscription(string routeName, string messageName, BindingType bindingType)
        {
            RouteName = routeName.ToLower();
            MessageName = messageName.ToLower();
            BindingType = bindingType;
        }

        public string RouteName { get; }
        public string MessageName { get; }
        public BindingType BindingType { get; }
    }
}