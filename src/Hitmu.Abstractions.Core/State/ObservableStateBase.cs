using Hitmu.Abstractions.Core.Messaging.Events;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hitmu.Abstractions.Core.State
{
    public abstract class ObservableStateBase<TIdType> : IState<TIdType>, IObservableState
        where TIdType : struct
    {
        private readonly Lazy<List<IEvent>> _events = new Lazy<List<IEvent>>(() => new List<IEvent>());

        public abstract TIdType Id { get; }

        public IEnumerable<IEvent> Events => _events.Value;

        public void ClearEvents() => _events.Value.Clear();

        public void RaiseEvent(IEvent @event)
        {
            if (_events.Value.Any(p => p.Id == @event.Id)) return;
            _events.Value.Add(@event);
        }
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is ObservableStateBase<TIdType> comparableObject) return Equals(comparableObject);
            return false;
        }

        protected bool Equals(ObservableStateBase<TIdType> other)
        {
            return Id.Equals(other.Id);
        }
    }
}