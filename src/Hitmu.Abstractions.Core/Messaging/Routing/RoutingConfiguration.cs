using Hitmu.Abstractions.Core.Messaging.Commands;
using Hitmu.Abstractions.Core.Messaging.Events;
using Hitmu.Abstractions.Core.Messaging.Metadata;
using Hitmu.Abstractions.Core.Results;
using System.Collections.Generic;
using System.Linq;

namespace Hitmu.Abstractions.Core.Messaging.Routing
{
    public abstract class RoutingConfiguration<TProvider> where TProvider : class
    {
        private readonly Dictionary<string, MessageRoute> _messageRoutes;
        private readonly Dictionary<string, MessageSubscription> _messageSubscriptions;

        protected RoutingConfiguration()
        {
            _messageRoutes = new Dictionary<string, MessageRoute>();
            _messageSubscriptions = new Dictionary<string, MessageSubscription>();
        }

        public bool ShouldCreateTopology { get; protected set; }
        public bool ShouldCreateServiceInboundConsumer { get; protected set; }

        protected string ParseToMessageNamePattern(string messageName)
        {
            return messageName.ToLower();
        }

        private RoutingConfiguration<TProvider> ConfigureRouting(string endpointName, string messageName,
            BindingType bindingType)
        {
            _messageRoutes.Add(
                messageName.ToLower(),
                new MessageRoute(endpointName, messageName.ToLower(), bindingType));

            return this;
        }

        private RoutingConfiguration<TProvider> ConfigureSubscription(string endpointName, string messageName,
            BindingType bindingType)
        {
            _messageSubscriptions.Add(
                messageName.ToLower(),
                new MessageSubscription(endpointName, messageName.ToLower(), bindingType));

            return this;
        }

        public RoutingConfiguration<TProvider> CreateTopology()
        {
            ShouldCreateTopology = true;
            return this;
        }

        public RoutingConfiguration<TProvider> CreateServiceInboundConsumer()
        {
            ShouldCreateServiceInboundConsumer = true;
            return this;
        }

        public RoutingConfiguration<TProvider> Publish<TEvent>(string publisherName) where TEvent : IEvent
        {
            return ConfigureRouting(publisherName, typeof(TEvent).Name, BindingType.Event);
        }

        public RoutingConfiguration<TProvider> Subscribe<TEvent>(string listenerName) where TEvent : IEvent
        {
            return ConfigureSubscription(listenerName, typeof(TEvent).Name, BindingType.Event);
        }

        public RoutingConfiguration<TProvider> RouteTo<TCommand, TCommandResult>(string endpointName)
            where TCommand : ICommand<TCommandResult>
            where TCommandResult : ICommandResult
        {
            return ConfigureRouting(endpointName, typeof(TCommand).Name, BindingType.Command);
        }

        public IEnumerable<MessageSubscription> GetEventSubscriptions()
        {
            return _messageSubscriptions.Values;
        }

        public IEnumerable<MessageRoute> GetEventPublications()
        {
            return _messageRoutes.Values.Where(p => p.BindingType == BindingType.Event);
        }

        public IEnumerable<MessageRoute> GetRoutedCommands()
        {
            return _messageRoutes.Values.Where(p => p.BindingType == BindingType.Command);
        }

        public Result<MessageRoute> GetRouteToMessage(string messageName)
        {
            var message = ParseToMessageNamePattern(messageName);
            return _messageRoutes.ContainsKey(message)
                ? Result<MessageRoute>.From(_messageRoutes[message])
                : Result<MessageRoute>.Error<MessageRoute>(new ErrorMessage($"{messageName} not found!"));
        }

        public Result<MessageRoute> GetRouteToMessage<TMessage>(TMessage message)
        {
            return message == null
                ? ErrorMessage.Error($"Argument {nameof(message)} not found.")
                : GetRouteToMessage(message.GetType().Name);
        }
    }
}