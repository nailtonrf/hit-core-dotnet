using Hitmu.Abstractions.Core.Messaging.Events;
using System.Collections.Generic;
using System.Linq;

namespace Hitmu.Abstractions.Core.State
{
    public abstract class EventStateBase<TIdType> : IState<TIdType>, IObservableState where TIdType : struct
    {
        private readonly List<IEvent> _events;

        public abstract TIdType Id { get; }

        protected EventStateBase(List<IEvent> events)
        {
            _events = events;
        }

        public IEnumerable<IEvent> Events => _events;

        public void ClearEvents() => _events.Clear();

        public void RaiseEvent(IEvent @event)
        {
            if (_events.Any(p => p.Id == @event.Id)) return;
            _events.Add(@event);
        }
    }
}