using Hitmu.Abstractions.Core.Messaging.Events;
using System.Collections.Generic;

namespace Hitmu.Abstractions.Core.State
{
    /// <summary>
    ///     Add event state observability to an object.
    /// </summary>
    public interface IObservableState
    {
        IEnumerable<IEvent> Events { get; }
        void ClearEvents();
        void RaiseEvent(IEvent @event);
    }
}