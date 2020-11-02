using Hitmu.Abstractions.Core.Messaging;
using Hitmu.Abstractions.Core.Messaging.Events;
using System;
using System.Threading.Tasks;

namespace Hitmu.Abstractions.Interactions.Messaging
{
    public sealed class EventIterator : IEventIterator
    {
        private readonly IMessageCatcher _messageCatcher;

        public EventIterator(IMessageCatcher messageCatcher)
        {
            _messageCatcher = messageCatcher ?? throw new ArgumentNullException(nameof(messageCatcher));
        }

        public Task Publish<TEvent>(TEvent @event) where TEvent : IEvent
        {
            if (@event == null) throw new ArgumentNullException(nameof(@event));
            _messageCatcher.Push(@event);
            return Task.CompletedTask;
        }
    }
}